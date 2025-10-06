using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Monki.DAL.Models;

namespace Monki.DAL
{
	public class MonkiContext : IdentityDbContext<MonkiUser>
	{
		public MonkiContext(DbContextOptions<MonkiContext> options) : base(options)
		{
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Make Email unique
			builder.Entity<MonkiUser>()
				.HasIndex(u => u.Email)
				.IsUnique();

			// One-to-many relationship between MonkiUser and MonkiDeck
			builder.Entity<MonkiDeck>()
				.HasOne(d => d.User)
				.WithMany(u => u.Decks)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// One-to-many relationship between MonkiDeck and MonkiCard
			builder.Entity<MonkiCard>()
				.HasOne(c => c.Deck)
				.WithMany(d => d.Cards)
				.HasForeignKey(c => c.DeckId)
				.OnDelete(DeleteBehavior.Cascade);
		}

		public DbSet<MonkiDeck> Decks { get; set; } = null!;
		public DbSet<MonkiCard> Cards { get; set; } = null!;
	}
}
