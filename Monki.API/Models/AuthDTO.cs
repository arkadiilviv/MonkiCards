using Monki.DAL.Models;

namespace Monki.API.Models
{
	public class RegisterDto
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
	}
	public class LoginDto
	{
		public string UserName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
	public class UserDTOResponse
	{
		public UserDTOResponse(MonkiUser user)
		{
			UserName = user.UserName;
			Email = user.Email;
			CreatedAt = user.CreatedAt;
			if (user.IsDeleted == true)
			{
				IsActive = false;
			} else
			{
				IsActive = user.IsActive;
			}
		}

		public string UserName { get; set; }
		public string Email { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
