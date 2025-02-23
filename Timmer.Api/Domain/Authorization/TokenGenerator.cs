namespace Timmer.Api.Domain.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Constant;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using User;

public class TokenGenerator(IConfiguration configuration) : ITokenGenerator {
	public string GenerateToken(User user) {
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = configuration["Jwt:Secret"]!.Select(Convert.ToByte).ToArray();

		var claims = new List<Claim> {
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Email, user.Email),
			new(ClaimTypes.Role, user.Role.GetDisplayName())
		};
		var tokenDescriptor = new SecurityTokenDescriptor {
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(60),
			Issuer = configuration["Jwt:Issuer"],
			Audience = configuration["Jwt:Audience"],
			SigningCredentials =
				new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}
