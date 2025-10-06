using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monki.API.Models;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using System.Threading.Tasks;

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

		/// <summary>
		/// Registers a new user.
		/// </summary>
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			try
			{
				var user = new MonkiUser { UserName = dto.UserName, Email = dto.Email };
				var res = await _userService.AddAsync(user, dto.Password);
				return res.Success ? Ok(res) : BadRequest(res);
			} catch (Exception ex)
			{
				// Handle known exception
				return BadRequest(ServiceResult.FailureResult(ex.Message, ex));
			}
		}

		/// <summary>
		/// Authenticates a user and returns a JWT token.
		/// </summary>
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			try
			{
				var findResult = await _userService.FindByUsername(dto.UserName);
				if (!findResult.Success)
					return Unauthorized(findResult);

				var user = findResult.Data as MonkiUser;
				var passwordCheck = await _userService.CheckPasswordAsync(user!, dto.Password);
				if (!passwordCheck.Success)
					return Unauthorized(passwordCheck);

				var jwtIssuer = _config["Jwt:Issuer"] ?? string.Empty;
				var jwtKey = _config["Jwt:Key"] ?? string.Empty;
				var tokenRes = _userService.IssueToken(user!, jwtIssuer, jwtKey);

				return tokenRes.Success ? Ok(tokenRes) : BadRequest(tokenRes);
			} catch (Exception ex)
			{
				// Handle known exception
				return BadRequest(ServiceResult.FailureResult(ex.Message, ex));
			}
		}

		[HttpGet("ping")]
		[Authorize]
		public async Task<IActionResult> AuthPing()
		{
			return Ok(ServiceResult.SuccessResult("Pong"));
		}
	}
}
