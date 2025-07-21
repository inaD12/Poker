using Microsoft.EntityFrameworkCore;
using Poker.Users.Domain.Entities;

namespace Poker.Users.Infrastructure.Features.DBContexts;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("users");
    }
}