using Microsoft.EntityFrameworkCore;

namespace Monki.DAL
{
	public class MonkiContex : DbContext
	{
		public MonkiContex(DbContextOptions<MonkiContex> options) : base(options)
		{
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}
	}
}
