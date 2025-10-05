using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Monki.API.Models;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using Monki.DAL.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens;
namespace Monki.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IConfiguration _config;
		public AuthController(IUserService userService, IConfiguration config)
		{
			_userService = userService;
			_config = config;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var user = new MonkiUser { UserName = dto.UserName, Email = dto.Email };
			var res = await _userService.AddAsync(user, dto.Password);

			if (res.Success)
			{
				return Ok(res);
			} else
			{
				return BadRequest(res);
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var findResult = await _userService.FindByUsername(dto.UserName);
			if (findResult.Success == false)
				return Unauthorized(findResult);
			MonkiUser user = (MonkiUser)findResult.Data!;
			var passwordCheck = await _userService.CheckPasswordAsync(user, dto.Password);

			if (passwordCheck.Success == false)
				return Unauthorized(passwordCheck);

			string jwtIssuer = _config["Jwt:Issuer"]!;
			string jwtKey = _config["Jwt:Key"]!;
			var tokenRes = _userService.IssueToken(user, jwtIssuer, jwtKey);

			if (tokenRes.Success)
			{
				return Ok(tokenRes);
			} else
			{
				return BadRequest(tokenRes);
			}
		}
	}
}
