using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Models;
using Poker.Game.Domain.Entities;

namespace Poker.Game.Domain.Abstractions.Interfaces;

public interface ILobbyRepository : IRepository<Lobby>
{
    Task<PagedList<Lobby>> GetAllLobbiesPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}