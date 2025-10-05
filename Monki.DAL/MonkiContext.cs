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
		}
	}
}
