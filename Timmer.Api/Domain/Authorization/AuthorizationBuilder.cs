namespace Timmer.Api.Domain.Authorization;

using System.Security.Claims;
using Constant;
using Microsoft.IdentityModel.Tokens;

public static class AuthorizationBuilder {
	public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration) {
		var key = configuration["Jwt:Secret"]!.Select(Convert.ToByte).ToArray();
		services.AddSingleton<ITokenGenerator, TokenGenerator>();
		services.AddAuthorizationBuilder()
			.AddPolicy(RoleValues.Admin, policy => {
				policy.RequireClaim(ClaimTypes.Role, RoleValues.Admin);
			});
		services.AddAuthentication()
			.AddJwtBearer(opt => {
				opt.TokenValidationParameters = new TokenValidationParameters {
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ValidAudience = configuration["Jwt:Audience"]!,
					ValidIssuer = configuration["Jwt:Issuer"]!
				};
			});
		return services;
	}
}
