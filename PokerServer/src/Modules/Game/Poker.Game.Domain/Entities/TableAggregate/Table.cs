using Poker.Common.Domain;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Enums;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Enums;
using Poker.Game.Domain.Events;
using Poker.Game.Domain.Responses;
using Poker.Game.Domain.Services;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Domain.Entities.TableAggregate;

public sealed class Table : Entity
{
#pragma warning disable CS8618
    private Table()
    {
    }
#pragma warning restore CS8618
    private Table(
        List<Card> communityCards,
        int currentPot,
        List<Player> players,
        int currentTurnPlayerPosition,
        int dealerPosition,
        GamePhase phase,
        int currentBet,
        int minimumRaise,
        Deck deck)
    {
        CommunityCards = communityCards;
        CurrentPot = currentPot;
        DealerPosition = dealerPosition;
        CurrentPhase = phase;
        CurrentBet = currentBet;
        MinimumRaise = minimumRaise;
        Deck = deck;
        PlayerManager = new PlayerManager(players, currentTurnPlayerPosition);
    }

    public List<Card> CommunityCards { get; }
    public int CurrentPot { get; private set; }
    public int DealerPosition { get; private set; }
    public int CurrentBet { get; private set; }
    public int MinimumRaise { get; }
    public GamePhase CurrentPhase { get; private set; }
    public Deck Deck { get; }
    public PlayerManager PlayerManager { get; }

    public static Result<Table> StartGame(List<Player> players)
    {
        switch (players.Count)
        {
            case > 6:
                return Result<Table>.Failure(ResponseList.SixPlayersMaximum);
            case < 2:
                return Result<Table>.Failure(ResponseList.TwoPlayersRequired);
        }

        var shuffledDeck = Deck.CreateShuffled();

        foreach (var player in players)
        {
            var hand = Hand.Create([shuffledDeck.Draw(), shuffledDeck.Draw()]);
            player.SetHand(hand);
        }

        var gameRoom = new Table(
            new List<Card>(),
            0,
            players,
            0,
            0,
            GamePhase.PreFlop,
            0,
            10,
            shuffledDeck);

        return Result<Table>.Success(gameRoom);
    }

    public Result PlayerPlaceBet(string playerId, int amount)
    {
        var playerResult = PlayerManager.GetPlayerIfHisTurn(playerId);
        if (playerResult.IsFailure)
            return Result.Failure(playerResult.Response);

        if (amount < 0)
            return Result.Failure(ResponseList.AmountCantBeNegative);

        var player = playerResult.Value!;

        var toCall = CurrentBet - player.Hand!.Bet;
        if (amount < toCall)
            return Result.Failure(ResponseList.BetTooSmall);

        if (amount > toCall)
        {
            var raiseAmount = amount - toCall;
            if (raiseAmount < MinimumRaise)
                return Result.Failure(ResponseList.MinimumRaiseNotMet);
        }

        if (player.Balance < amount)
            return Result.Failure(ResponseList.InsufficientFunds);

        var result = player.Hand.AddToBet(amount);
        if (result.IsFailure)
            return result;

        CurrentPot += amount;
        player.RemoveFromBalance(amount);
        CurrentBet = player.Hand.Bet;

        AdvanceTurn();

        RaiseDomainEvent(new PlayerTookActionDomainEvent(
            Id,
            playerId,
            PlayerActionType.PlaceBet,
            PlayerManager.GetCurrentTurnPlayer().Id,
            amount));

        return Result.Success();
    }

    public Result PlayerCheck(string playerId)
    {
        var playerResult = PlayerManager.GetPlayerIfHisTurn(playerId);
        if (playerResult.IsFailure)
            return Result.Failure(playerResult.Response);

        if (playerResult.Value!.Hand!.Bet != CurrentBet)
            return Result.Failure(ResponseList.MustMatchBet);

        AdvanceTurn();

        RaiseDomainEvent(new PlayerTookActionDomainEvent(
            Id,
            playerId,
            PlayerActionType.Check,
            PlayerManager.GetCurrentTurnPlayer().Id));

        return Result.Success();
    }

    public Result PlayerFold(string playerId)
    {
        var playerResult = PlayerManager.GetPlayerIfHisTurn(playerId);
        if (playerResult.IsFailure)
            return Result.Failure(playerResult.Response);

        var player = playerResult.Value!;

        var result = player.Hand!.Fold();
        if (result.IsFailure)
            return result;

        AdvanceTurn();

        RaiseDomainEvent(new PlayerTookActionDomainEvent(
            Id,
            playerId,
            PlayerActionType.Fold,
            PlayerManager.GetCurrentTurnPlayer().Id));

        return Result.Success();
    }

    public Result PlayerAllIn(string playerId)
    {
        var playerResult = PlayerManager.GetPlayerIfHisTurn(playerId);
        if (playerResult.IsFailure)
            return Result.Failure(playerResult.Response);

        var player = playerResult.Value!;
        var hand = player.Hand;

        if (player.Balance <= 0)
            return Result.Failure(ResponseList.InsufficientFunds);

        var result = hand!.AllIn(player.Balance);
        if (result.IsFailure)
            return result;

        var playerBet = player.Balance;

        CurrentPot += playerBet;
        player.RemoveFromBalance(playerBet);

        if (hand.Bet > CurrentBet)
            CurrentBet = hand.Bet;

        AdvanceTurn();

        RaiseDomainEvent(new PlayerTookActionDomainEvent(
            Id,
            playerId,
            PlayerActionType.AllIn,
            PlayerManager.GetCurrentTurnPlayer().Id,
            playerBet));

        return Result.Success();
    }

    public GameStateDto GetGameState(string requestingPlayerId)
    {
        var players = PlayerManager.Players
            .Select(p => new PlayerStateDto(
                p.Id,
                p.Balance,
                p.Hand?.IsFolded ?? false,
                p.Hand?.IsAllIn ?? false,
                p.Hand?.Bet ?? 0,
                PlayerManager.IsPlayerTurn(p.Id),
                p.Id == requestingPlayerId
                    ? p.Hand?.Cards.Select(c => new CardDto(c.Suit, c.Rank)).ToList()
                    : null
            ))
            .ToList();

        return new GameStateDto(
            CurrentPhase,
            CommunityCards.Select(c => new CardDto(c.Suit, c.Rank)).ToList(),
            CurrentPot,
            CurrentBet,
            MinimumRaise,
            PlayerManager.GetCurrentTurnPlayer().Id,
            players
        );
    }

    public PlayerInfoDto? GetPlayerDto(string playerId)
    {
        return PlayerManager.Players
            .FirstOrDefault(p => p.Id == playerId)?
            .ToDto();
    }

    private void AdvanceTurn()
    {
        if (PlayerManager.OnlyOneActivePlayer())
        {
            HandleShowdown();
        }
        else if (PlayerManager.IsBettingRoundComplete(CurrentBet))
        {
            var cards = AdvancePhase();
            CommunityCards.AddRange(cards);

            var cardDtos = cards.Select(c => new CardDto(c.Suit, c.Rank)).ToList();

            RaiseDomainEvent(new GamePhaseUpdatedDomainEvent(
                Id,
                CurrentPhase,
                cardDtos));

            PlayerManager.ResetHandsForNextRound();
            CurrentBet = 0;
            PlayerManager.SetFirstActivePlayer();
        }
        else
        {
            PlayerManager.GetNextActivePosition();
        }
    }

    private List<Card> AdvancePhase()
    {
        switch (CurrentPhase)
        {
            case GamePhase.PreFlop:
                CurrentPhase = GamePhase.Flop;
                return
                [
                    Deck.Draw(),
                    Deck.Draw(),
                    Deck.Draw()
                ];
            case GamePhase.Flop:
                CurrentPhase = GamePhase.Turn;
                return [Deck.Draw()];
            case GamePhase.Turn:
                CurrentPhase = GamePhase.River;
                return [Deck.Draw()];
            case GamePhase.River:
                HandleShowdown();
                break;
            default:
                throw new InvalidOperationException("Unknown phase.");
        }

        return new List<Card>();
    }

    private void HandleShowdown()
    {
        // TODO: Reset game.

        CurrentPhase = GamePhase.Showdown;

        var players = PlayerManager.Players;

        var activePlayers = players
            .Where(p => !p.Hand!.IsFolded)
            .ToList();

        if (activePlayers.Count == 1)
        {
            var winner = activePlayers[0];
            winner.AddToBalance(CurrentPot);
            return;
        }

        var evaluated = activePlayers
            .Select(p => new
            {
                Player = p,
                Score = HandEvaluator.EvaluateHand(p.Hand!.Cards.Concat(CommunityCards).ToList())
            })
            .OrderByDescending(x => x.Score)
            .ToList();

        var topScore = evaluated.First().Score;
        var winners = evaluated.Where(x => x.Score == topScore).ToList();

        var share = CurrentPot / winners.Count();

        foreach (var winner in winners) winner.Player.AddToBalance(share);

        RaiseDomainEvent(new ShowdownDomainEvent(
            Id,
            winners.Select(w => w.Player.Id).ToList(),
            share));
    }
}