using Microsoft.EntityFrameworkCore;
using Poker.Users.Domain.Entities;

namespace Poker.Users.Infrastructure.Features.DBContexts;

public class UsersDBContext : DbContext
{
	public DbSet<User> Users { get; set; }

	public UsersDBContext(DbContextOptions<UsersDBContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// modelBuilder.AddInboxStateEntity();
		// modelBuilder.AddOutboxMessageEntity();
		// modelBuilder.AddOutboxStateEntity();
	}
}
