using Microsoft.AspNetCore.Identity;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Monki.DAL.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<MonkiUser> _manager;
		public UserService(UserManager<MonkiUser> manager)
		{
			_manager = manager;
		}

		public async Task<ServiceResult> AddAsync(MonkiUser user, string password)
		{
			if (await _manager.FindByEmailAsync(user.Email) != null)
			{
				return ServiceResult.FailureResult("Email is already taken.");
			}

			IdentityResult res;
			try
			{
				res = await _manager.CreateAsync(user, password);
			} catch (Exception ex)
			{
				return ServiceResult.FailureResult("User creation failed due to an exception.", ex.Message);
			}


			if (res.Succeeded)
			{
				return ServiceResult.SuccessResult("User created successfully.", user);
			} else
			{
				return ServiceResult.FailureResult("User creation failed.", res.Errors);
			}
		}

		public async Task<ServiceResult> FindByUsername(string username)
		{
			var res = await _manager.FindByNameAsync(username);

			if (res != null && res.IsActive && !res.IsDeleted)
			{
				return ServiceResult.SuccessResult("Ok", res);
			} else if (res != null && (!res.IsActive || res.IsDeleted))
			{
				return ServiceResult.FailureResult("User is inactive or deleted.");
			} else
			{
				return ServiceResult.FailureResult("User not found.");
			}
		}

		public async Task<ServiceResult> CheckPasswordAsync(MonkiUser user, string password)
		{
			var res = await _manager.CheckPasswordAsync(user, password);

			if (res)
			{
				return ServiceResult.SuccessResult("Password is correct.");
			} else
			{
				return ServiceResult.FailureResult("Password is incorrect.");
			}
		}

		public ServiceResult IssueToken(MonkiUser user, string jwtIssuer, string jwtKey)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email!),
				new Claim(JwtRegisteredClaimNames.Nickname, user.UserName!)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: jwtIssuer,
				claims: claims,
				expires: DateTime.Now.AddHours(1),
				signingCredentials: creds);

			return ServiceResult.SuccessResult("Token issued successfully.", new JwtSecurityTokenHandler().WriteToken(token));
		}

		public async Task<ServiceResult> GetUserAsync(ClaimsPrincipal user)
		{
			var monkiUser = await _manager.GetUserAsync(user);
			if(monkiUser == null)
				return ServiceResult.FailureResult("User not found.");
			return ServiceResult.SuccessResult(data: monkiUser);
		}
	}
}
