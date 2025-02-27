namespace Timmer.Api.Domain.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Configuration;
using Dto;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using Timmer.Domain.User;

public class TokenGenerator(IConfiguration configuration) : ITokenGenerator {
	public TokenDto GenerateToken(UserModel user, bool isRefreshToken) => GenerateToken(user, isRefreshToken? 7 : 1);

	private TokenDto GenerateToken(UserModel user, int expiresIn) {
		var tokenHandler = new JwtSecurityTokenHandler();
		var jwtConfiguration = new JwtConfiguration(configuration);
		var claims = new List<Claim> {
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Email, user.Email),
			new(ClaimTypes.Role, user.Role.GetDisplayName())
		};
		var expiresAt = DateTime.UtcNow.AddDays(expiresIn);
		var tokenDescriptor = new SecurityTokenDescriptor {
			Subject = new ClaimsIdentity(claims),
			Expires = expiresAt,
			Issuer = jwtConfiguration.ValidIssuer,
			Audience = jwtConfiguration.ValidAudience,
			SigningCredentials =
				new SigningCredentials(new SymmetricSecurityKey(jwtConfiguration.Key),
					SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return new TokenDto(tokenHandler.WriteToken(token), expiresAt);
	}
}
