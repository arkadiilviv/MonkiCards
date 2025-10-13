using Monki.DAL.Models;
using System.Security.Claims;
namespace Monki.DAL.Interfaces
{
	public interface IUserService
	{
		public Task<ServiceResult> AddAsync(MonkiUser user, string password);
		public Task<ServiceResult> FindByUsername(string username);
		public Task<ServiceResult> CheckPasswordAsync(MonkiUser user, string password);
		public ServiceResult IssueToken(MonkiUser user, string jwtIssuer, string jwtKey);
		public Task<ServiceResult> GetUserAsync(ClaimsPrincipal user);
		public IEnumerable<MonkiUser> GetAll();
	}
}
