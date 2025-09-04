using Microsoft.EntityFrameworkCore;
using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Infrastructure.Features.Snapshots;

namespace Poker.Game.Infrastructure.Features.DBContexts;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<TableSnapshot> TableSnapshots { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }
    public DbSet<Player> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tables");

        modelBuilder.Entity<TableSnapshot>()
            .Property(e => e.TableJson)
            .HasColumnName("TableJson");

        modelBuilder.Entity<Player>()
            .Property<string>("LobbyId");

        modelBuilder.Ignore<Hand>();

        modelBuilder.Entity<Lobby>()
            .HasMany(l => l.Players)
            .WithOne()
            .HasForeignKey("LobbyId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}