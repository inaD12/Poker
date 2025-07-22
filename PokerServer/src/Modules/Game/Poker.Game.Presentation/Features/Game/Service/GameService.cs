using MediatR;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Game.Commands.GameStart;
using Poker.Game.Application.Features.Game.Commands.PlayerAllIn;
using Poker.Game.Application.Features.Game.Commands.PlayerChecked;
using Poker.Game.Application.Features.Game.Commands.PlayerFolded;
using Poker.Game.Application.Features.Game.Commands.PlayerPlacedBet;
using Poker.Game.Application.Features.Game.Commands.StartNextHand;
using Poker.Game.Application.Features.Game.Models;
using Poker.Game.Application.Features.Game.Queries.GetTable;

namespace Poker.Game.Presentation.Features.Game.Service;

internal class GameService : IGameService
{
    private readonly ISender _sender;

    public GameService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result<GameCommandViewModel>> StartGameAsync(string lobbyId, CancellationToken cancellationToken)
    {
        var command = new GameStartCommand(lobbyId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    public async Task<Result> PlayerAllInAsync(string tableId, string playerId, CancellationToken cancellationToken)
    {
        var command = new PlayerAllInCommand(tableId, playerId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    public async Task<Result> PlayerCheckedAsync(string tableId, string playerId, CancellationToken cancellationToken)
    {
        var command = new PlayerCheckedCommand(tableId, playerId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    public async Task<Result> PlayerFoldedAsync(string tableId, string playerId, CancellationToken cancellationToken)
    {
        var command = new PlayerFoldedCommand(tableId, playerId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    public async Task<Result> PlayerPlacedBetAsync(string tableId, string playerId, int amount,
        CancellationToken cancellationToken)
    {
        var command = new PlayerPlacedBetCommand(tableId, playerId, amount);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }
    
    public async Task<Result> StartNextHandAsync(string tableId, string playerId, CancellationToken cancellationToken)
    {
        var command = new StartNextHandCommand(tableId, playerId);
        
        var result = await _sender.Send(command, cancellationToken);

        return result;
    }
    
    public async Task<Result<GameStateDto>> GetTableAsync(string tableId, string playerId, CancellationToken cancellationToken)
    {
        var query = new GetTableQuery(tableId, playerId);
        
        var result = await _sender.Send(query,  cancellationToken);

        return result;
    }
}