using Newtonsoft.Json;
using Poker.Common.Domain;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Enums;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Enums;
using Poker.Game.Domain.Events;
using Poker.Game.Domain.Responses;
using Poker.Game.Domain.Services;

namespace Poker.Game.Domain.Entities.TableAggregate;

public sealed class Table : Entity
{
#pragma warning disable CS8618
    private Table()
    {
    }
#pragma warning restore CS8618
    [JsonConstructor]
    private Table(
        List<Card> communityCards,
        int currentPot,
        List<Player> players,
        int currentTurnPlayerPosition,
        int dealerPosition,
        GamePhase currentPhase,
        int currentBet,
        int minimumRaise,
        string hostPlayerId,
        Deck deck,
        bool waitingForNextHand,
        string id,
        DateTime createdAt,
        HashSet<string> playersWhoActed
        )
    {
        CommunityCards = communityCards;
        CurrentPot = currentPot;
        CurrentPhase = currentPhase;
        CurrentBet = currentBet;
        MinimumRaise = minimumRaise;
        Deck = deck;
        WaitingForNextHand = waitingForNextHand;
        _playerManager = new PlayerManager(players, currentTurnPlayerPosition, hostPlayerId, dealerPosition, playersWhoActed);
        Id = id;
        CreatedAt = createdAt;
    }

    
    private Table(
        List<Card> communityCards,
        int currentPot,
        List<Player> players,
        int currentTurnPlayerPosition,
        int dealerPosition,
        GamePhase currentPhase,
        int currentBet,
        int minimumRaise,
        string hostPlayerId,
        Deck deck,
        HashSet<string> playersWhoActed
        )
    {
        CommunityCards = communityCards;
        CurrentPot = currentPot;
        CurrentPhase = currentPhase;
        CurrentBet = currentBet;
        MinimumRaise = minimumRaise;
        Deck = deck;
        WaitingForNextHand = false;
        _playerManager = new PlayerManager(players, currentTurnPlayerPosition,  hostPlayerId, dealerPosition, playersWhoActed);
    }

    public List<Card> CommunityCards { get; }
    public int CurrentPot { get; private set; }
    public int CurrentBet { get; private set; }
    public int MinimumRaise { get; }
    public bool WaitingForNextHand { get; private set; }
    public GamePhase CurrentPhase { get; private set; }
    public Deck Deck { get; private set; }
    public string HostPlayerId => _playerManager.HostPlayerId;
    public IReadOnlyCollection<Player> Players => _playerManager.Players;
    public int CurrentTurnPlayerPosition => _playerManager.CurrentTurnPlayerPosition;
    public int DealerPosition => _playerManager.DealerPosition;
    public HashSet<string> PlayersWhoActed => _playerManager.PlayersWhoActed;
    
    
    private readonly PlayerManager _playerManager;

    public static Result<Table> StartGame(List<Player> players, string hostPlayerId)
    {
        if (players.All(p => p.Id != hostPlayerId))
            return Result<Table>.Failure(ResponseList.HostNotFromPlayers);
        
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
            hostPlayerId,
            shuffledDeck,
            new HashSet<string>());

        return Result<Table>.Success(gameRoom);
        }

    public Result PlayerPlaceBet(string playerId, int amount)
    {
        var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
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

        _playerManager.MarkPlayerActed(playerId);
        AdvanceTurn();

        RaiseDomainEvent(new PlayerTookActionDomainEvent(
            Id,
            playerId,
            PlayerActionType.PlaceBet,
            _playerManager.CurrentTurnPlayer.Id,
            amount));

        return Result.Success();
    }

    public Result PlayerCheck(string playerId)
    {
        var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
        if (playerResult.IsFailure)
            return Result.Failure(playerResult.Response);

        if (playerResult.Value!.Hand!.Bet != CurrentBet)
            return Result.Failure(ResponseList.MustMatchBet);

        _playerManager.MarkPlayerActed(playerId);
        AdvanceTurn();

        RaiseDomainEvent(new PlayerTookActionDomainEvent(
            Id,
            playerId,
            PlayerActionType.Check,
            _playerManager.CurrentTurnPlayer.Id));

        return Result.Success();
    }

    public Result PlayerFold(string playerId)
    {
        var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
        if (playerResult.IsFailure)
            return Result.Failure(playerResult.Response);

        var player = playerResult.Value!;

        var result = player.Hand!.Fold();
        if (result.IsFailure)
            return result;

        _playerManager.MarkPlayerActed(playerId);
        AdvanceTurn();

        RaiseDomainEvent(new PlayerTookActionDomainEvent(
            Id,
            playerId,
            PlayerActionType.Fold,
            _playerManager.CurrentTurnPlayer.Id));

        return Result.Success();
    }

    public Result PlayerAllIn(string playerId)
    {
        var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
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

        _playerManager.MarkPlayerActed(playerId);
        AdvanceTurn();

        RaiseDomainEvent(new PlayerTookActionDomainEvent(
            Id,
            playerId,
            PlayerActionType.AllIn,
            _playerManager.CurrentTurnPlayer.Id,
            playerBet));

        return Result.Success();
    }
    
    public Result KickPlayer(string playerId)
    {
        var result = _playerManager.KickPlayer(playerId);
        if(result.IsFailure)
            return result;

        if (_playerManager.ActivePlayerCount < 2)
            HandleShowdown();

        RaiseDomainEvent(new PlayerKickedDomainEvent(Id, playerId));

        return Result.Success();
    }
    
    public Result PlayerLeave(string playerId)
    {
        Player? player = _playerManager.GetPlayer(playerId);
        if (player == null)
            return Result.Failure(ResponseList.PlayerNotInGame);
        
        if(WaitingForNextHand || !player.Hand!.IsFolded || !player.Hand!.IsFolded)
            return Result.Failure(ResponseList.CannotLeaveDuringActiveHand);
        
        PlayerDisconnected(playerId);
        
        return Result.Success();
    }
    
    public Result PlayerDisconnected(string playerId)
    {
         var disconnectResult = _playerManager.PlayerDisconnected(playerId);
         if (disconnectResult.IsFailure)
             return disconnectResult;

         var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
         if (playerResult.IsSuccess)
         {
             var player = playerResult.Value!;

             player.Hand!.Fold();
             
            AdvanceTurn();
         }
    
         RaiseDomainEvent(new PlayerTookActionDomainEvent(
             Id,
             playerId,
             PlayerActionType.Disconnect,
             _playerManager.CurrentTurnPlayer.Id));

         return Result.Success();
    }
    
    public Result PlayerReconnected(string playerId)
    {
        var result = _playerManager.PlayerReconnected(playerId);
        if (result.IsFailure)
            return result;

        RaiseDomainEvent(new PlayerTookActionDomainEvent(
            Id,
            playerId,
            PlayerActionType.Reconnected,
            _playerManager.CurrentTurnPlayer.Id));
        
        return Result.Success();
    }

    public Result<GameStateDto> GetGameState(string requestingPlayerId)
    {
        if (_playerManager.Players.All(p => p.Id != requestingPlayerId))
            return Result<GameStateDto>.Failure(ResponseList.PlayerNotInGame);
        
        var players = _playerManager.Players
            .Select(p => new PlayerStateDto(
                p.Id,
                p.Username,
                p.Balance,
                p.Hand?.IsFolded ?? false,
                p.Hand?.IsAllIn ?? false,
                p.Hand?.Bet ?? 0,
                _playerManager.IsPlayerTurn(p.Id),
                p.IsDisconnected,
                p.Id == requestingPlayerId
                    ? true
                    : false,
                p.Id == requestingPlayerId
                    ? p.Hand?.Cards.Select(c => new CardDto(c.Suit, c.Rank)).ToList()
                    : null
            ))
            .ToList();

        var dto = new GameStateDto(
            CurrentPhase,
            CommunityCards.Select(c => new CardDto(c.Suit, c.Rank)).ToList(),
            CurrentPot,
            CurrentBet,
            MinimumRaise,
            _playerManager.CurrentTurnPlayer.Id,
            _playerManager.Dealer.Id,
            _playerManager.HostPlayerId,
            players);
            
        return Result<GameStateDto>.Success(dto);
    }
    
    public Result StartNextHand(string playerId)
    {
        if (playerId != _playerManager.HostPlayerId)
            return Result.Failure(ResponseList.OnlyHostCanStartNextHand);

        if (!WaitingForNextHand)
            return Result.Failure(ResponseList.HandNotFinished);

        var players = _playerManager.Players;
        
        switch (players.Count)
        {
            case > 6:
                return Result.Failure(ResponseList.SixPlayersMaximum);
            case < 2:
                return Result.Failure(ResponseList.TwoPlayersRequired);
        }

        var shuffledDeck = Deck.CreateShuffled();

        foreach (var player in players)
        {
            var hand = Hand.Create([shuffledDeck.Draw(), shuffledDeck.Draw()]);
            player.SetHand(hand);
        }

        CommunityCards.Clear();
        CurrentPot = 0;
        CurrentBet = 0;
        WaitingForNextHand = false;

        CurrentPhase = GamePhase.PreFlop;
        Deck = shuffledDeck;
        _playerManager.DealerPosition = ( _playerManager.DealerPosition + 1) % players.Count;
        _playerManager.SetFirstActivePlayer();

        RaiseDomainEvent(new NewHandDomainEvent(Id));
        return Result.Success();
    }

    private void AdvanceTurn()
    {
        if (_playerManager.ActivePlayerCount < 2)
        {
            HandleShowdown();
        }
        else if (_playerManager.IsBettingRoundComplete(CurrentBet))
        {
            var cards = AdvancePhase();
            CommunityCards.AddRange(cards);

            var cardDtos = cards.Select(c => new CardDto(c.Suit, c.Rank)).ToList();

            RaiseDomainEvent(new GamePhaseUpdatedDomainEvent(
                Id,
                CurrentPhase,
                cardDtos));

            _playerManager.ResetHandsForNextRound();
            CurrentBet = 0;
            _playerManager.ResetPlayersActed();
            _playerManager.SetFirstActivePlayer();
        }
        else
        {
            _playerManager.SetNextActivePosition();
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
        WaitingForNextHand = true;
        CurrentPhase = GamePhase.Showdown;

        var players = _playerManager.Players;
        var activePlayers = players
            .Where(p => !p.Hand!.IsFolded)
            .ToList();

        Dictionary<string, decimal> earnings = new();

        if (activePlayers.Count == 1)
        {
            var winner = activePlayers[0];
            winner.AddToBalance(CurrentPot);
            earnings[winner.Id] = CurrentPot;

            RaiseDomainEvent(new ShowdownDomainEvent(
                Id,
                new List<string> { winner.Id },
                CurrentPot,
                GetPlayersStateDtosWithCards()));
        }
        else
        {
            var evaluated = activePlayers
                .Select(p => new
                {
                    Player = p,
                    Score = HandEvaluator.EvaluateHand(
                        p.Hand!.Cards.Concat(CommunityCards).ToList()
                    )
                })
                .OrderByDescending(x => x.Score)
                .ToList();

            var topScore = evaluated.First().Score;
            var winners = evaluated.Where(x => x.Score == topScore).ToList();

            var share = CurrentPot / winners.Count;

            foreach (var winner in winners)
            {
                winner.Player.AddToBalance(share);
                earnings[winner.Player.Id] = share;
            }

            RaiseDomainEvent(new ShowdownDomainEvent(
                Id,
                winners.Select(w => w.Player.Id).ToList(),
                share,
                GetPlayersStateDtosWithCards()));
        }

        foreach (var player in players)
        {
            var won = earnings.ContainsKey(player.Id);
            var playerEarnings = earnings.GetValueOrDefault(player.Id, 0);

            RaiseDomainEvent(new PlayerPlayedHandDomainEvent(
                player.Id,
                won,
                playerEarnings
            ));
        }
    }


    private List<PlayerStateDto> GetPlayersStateDtosWithCards()
    {
        var players = _playerManager.Players
            .Select(p => new PlayerStateDto(
                p.Id,
                p.Username,
                p.Balance,
                p.Hand?.IsFolded ?? false,
                p.Hand?.IsAllIn ?? false,
                p.Hand?.Bet ?? 0,
                _playerManager.IsPlayerTurn(p.Id),
                p.IsDisconnected, 
                false,
                p.Hand?.Cards.Select(c => new CardDto(c.Suit, c.Rank)).ToList()
            ))
            .ToList();
        
        return players;
    }
}