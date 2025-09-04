using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Infrastructure.Extensions;
using Poker.Game.Domain.Abstractions.Interfaces;
using Poker.Game.Infrastructure.Features.DBContexts;
using Poker.Game.Infrastructure.Features.Repositories;
using Poker.Game.Infrastructure.Features.UnitOfWork;

namespace Poker.Game.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddScoped<ITableRepository, TableRepository>()
            .AddScoped<ILobbyRepository, LobbyRepository>()
            .AddScoped<ITablesUnitOfWork, TablesUnitOfWork>();

        services
            .AddDateTimeProvider()
            .AddDatabaseContext<GameDbContext>(configuration, o => o.MigrationsHistoryTable(
                HistoryRepository.DefaultTableName,
                "tables"));


        return services;
    }
}