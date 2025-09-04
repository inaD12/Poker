using Microsoft.EntityFrameworkCore;
using Poker.Game.Infrastructure.Features.DBContexts;
using Poker.Users.Infrastructure.Features.DBContexts;

namespace PokerServer.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        await ApplyMigration<UsersDbContext>(scope);
        await ApplyMigration<GameDbContext>(scope);
    }

    private static async Task ApplyMigration<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        await using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
            await context.Database.MigrateAsync();
    }
}