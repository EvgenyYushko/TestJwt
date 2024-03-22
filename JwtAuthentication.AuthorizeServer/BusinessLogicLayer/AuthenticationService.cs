using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Model;
using JwtAuthentication.AuthorizeServer.ServiceLayer.Services;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.AuthorizeServer.BusinessLogicLayer
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly string SECRET_KEY = "test123dasdadasdasdasdasasdfasdfdas";
		private readonly IUserService _userService;
		private readonly int REFRESH_TOKEN_EXPIRY_MINUNES = 5;
		private readonly int ACCESS_TOKEN_EXPIRY_MINUNES = 1;

		public AuthenticationService(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<bool> Register(UserClient userClient)
		{
			var userDto = await _userService.FindByName(userClient.Username);
			if (userDto != null)
			{
				throw new Exception("User already exists.");
			}

			var newUser = new UserDto
			{
				UserName = userClient.Username
			};

			var isCreated = await _userService.Create(newUser, userClient.Password);
			if (!isCreated)
			{
				throw new Exception("Failed to create user");
			}

			return true;
		}

		public async Task<UserServer> Login(UserClient userClient)
		{
			var userDto = await _userService.FindByName(userClient.Username);

			if (userDto == null || !await _userService.CheckPassword(userDto, userClient.Password))
			{
				throw new Exception("Unauthorized");
			}

			var token = GenerateTokenForUser(userClient.Username, userDto);

			await _userService.Update(userDto);

			return new UserServer
			{
				AccessToken = token.accessToken,
				Expiration = token.validTo,
				RefreshToken = token.refreshToken
			};
		}

		public async Task<UserServer> Refresh(UserClient userClient)
		{
			var principal = GetPrincipalFromExpiredToken(userClient.AccessToken);

			if (principal?.Identity?.Name is null)
			{
				throw new Exception("Unauthorized. Access token invalid");
			}

			var user = await _userService.FindByName(principal.Identity.Name);

			if (user is null || user.TokenModel.RefreshToken != userClient.RefreshToken || user.TokenModel.RefreshTokenExpiry < DateTime.Now)
			{
				throw new Exception("Unauthorized");
			}

			var token = GenerateTokenForUser(principal.Identity.Name, user);

			await _userService.Update(user);

			return new UserServer
			{
				AccessToken = token.accessToken,
				Expiration = token.validTo,
				RefreshToken = token.refreshToken
			};
		}

		private (string accessToken, DateTime validTo, string refreshToken) GenerateTokenForUser(string userName, UserDto userDto)
		{
			var jwtToken = GenerateJwt(userName);
			var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			var refreshToken = GenerateRefreshToken();

			userDto.TokenModel.AccessToken = accessToken;
			userDto.TokenModel.AccessTokenExpiry = jwtToken.ValidTo;
			userDto.TokenModel.RefreshToken = refreshToken;
			userDto.TokenModel.RefreshTokenExpiry = DateTime.Now.AddMinutes(REFRESH_TOKEN_EXPIRY_MINUNES);

			return (accessToken, jwtToken.ValidTo, refreshToken);
		}

		public async Task<bool> Revoke(string token)
		{
			var principal = GetPrincipalFromExpiredToken(token);

			if (principal?.Identity?.Name is null)
			{
				throw new Exception("Unauthorized");
			}

			var user = await _userService.FindByName(principal.Identity.Name);

			user.TokenModel.AccessToken = null;
			user.TokenModel.RefreshToken = null;
			user.TokenModel.RefreshTokenExpiry = null;

			await _userService.Update(user);

			return true;
		}

		public async Task<bool> CheckToken(string authToken)
		{
			ClaimsPrincipal principal;
			try
			{
				principal = ValidateToken(authToken);
				var user = await _userService.FindByName(principal.Identity.Name);

				//todo доставать самый послений токен и сравнивать с текущим , если пришёл не последний значит злоумишлинник
				if (user.TokenModel.AccessToken is null)
				{
					throw new Exception("Non actual token");
				}
			}
			catch (SecurityTokenException e)
			{
				return false;
			}

			return principal?.Identity?.Name is not null;
		}

		private ClaimsPrincipal? ValidateToken(string token)
		{
			var secret = SECRET_KEY ?? throw new InvalidOperationException("Secret not configured");

			var validation = new TokenValidationParameters
			{
				ValidateLifetime = true,
				LifetimeValidator = (_, expires, _, _) => expires >= DateTime.Now,
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidIssuer = "Sample",
				ValidAudience = "Sample",
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
			};

			return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
		}

		private JwtSecurityToken GenerateJwt(string username)
		{
			var authClaims = new List<Claim>
			{
				new(ClaimTypes.Name, username),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

			var token = new JwtSecurityToken
			(
				issuer: "Sample",
				audience: "Sample",
				expires: DateTime.SpecifyKind(DateTime.Now.AddMinutes(ACCESS_TOKEN_EXPIRY_MINUNES), DateTimeKind.Utc),
				claims: authClaims,
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);

			return token;
		}

		private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
		{
			var validation = new TokenValidationParameters
			{
				ValidateLifetime = false, // Because there is no expiration in the generated token
				ValidateAudience = false, // Because there is no audiance in the generated token
				ValidateIssuer = false, // Because there is no issuer in the generated token
				ValidIssuer = "Sample",
				ValidAudience = "Sample",
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY))
			};

			return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
		}

		private static string GenerateRefreshToken()
		{
			var randomNumber = new byte[64];

			using var generator = RandomNumberGenerator.Create();

			generator.GetBytes(randomNumber);

			return Convert.ToBase64String(randomNumber);
		}
	}
}