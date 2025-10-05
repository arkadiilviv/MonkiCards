using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Monki.DAL;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;
using Monki.DAL.Services;
using System.Text;

namespace Monki.API
{
	public static class DiConfiguration
	{
		public static WebApplicationBuilder SetConfiguration(this WebApplicationBuilder builder)
		{

			// Add services to the container.
			builder.Services.AddControllers();
			// Add OpenApi
			builder.Services.AddOpenApi();
			// EF
			builder.Services.AddDbContextPool<MonkiContext>(opt =>
				opt.UseNpgsql(
					builder.Configuration.GetConnectionString("MonkiContext"),
					npgsqlOptions => npgsqlOptions.MigrationsAssembly("Monki.DAL")
				)
			);
			// Auth
			builder.Services.AddIdentity<MonkiUser, IdentityRole>()
				.AddEntityFrameworkStores<MonkiContext>()
				.AddDefaultTokenProviders();
			var jwtKey = builder.Configuration["Jwt:Key"]!;
			var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = "JwtBearer";
				options.DefaultChallengeScheme = "JwtBearer";
			})
			.AddJwtBearer("JwtBearer", options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtIssuer,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
				};
			});
			// Swagger
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			// Services
			builder.Services.AddScoped<IUserService, UserService>();
			return builder;
		}
	}
}
