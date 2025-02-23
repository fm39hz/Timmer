namespace Timmer.Api.Domain.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using User;

public class TokenGenerator(IConfiguration configuration) : ITokenGenerator {
	public string GenerateToken(User user) {
		var tokenHandler = new JwtSecurityTokenHandler();
		var jwtConfiguration = new JwtConfiguration(configuration);
		var claims = new List<Claim> {
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Email, user.Email),
			new(ClaimTypes.Role, user.Role.GetDisplayName())
		};
		var tokenDescriptor = new SecurityTokenDescriptor {
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(60),
			Issuer = jwtConfiguration.ValidIssuer,
			Audience = jwtConfiguration.ValidAudience,
			SigningCredentials =
				new SigningCredentials(new SymmetricSecurityKey(jwtConfiguration.Key),
					SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}
