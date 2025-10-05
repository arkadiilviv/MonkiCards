using Microsoft.AspNetCore.Identity;

namespace Monki.DAL.Models
{
	public class MonkiUser : IdentityUser
	{
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public bool IsActive { get; set; } = true;
		public bool IsDeleted { get; set; } = false;
	}
}
