namespace Timmer.Api.Domain.Authorization;

using System.Security.Claims;
using Configuration;
using Constant;
using Microsoft.IdentityModel.Tokens;

public static class AuthorizationBuilder {
	public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration) {
		var jwtConfiguration = new JwtConfiguration(configuration);
		services.AddSingleton<ITokenGenerator, TokenGenerator>();
		services.AddAuthorizationBuilder()
			.AddPolicy(RoleValues.ADMIN, policy => {
				policy.RequireClaim(ClaimTypes.Role, RoleValues.ADMIN);
				policy.RequireRole(RoleValues.ADMIN);
			})
			.AddPolicy(RoleValues.USER, policy => {
				policy.RequireClaim(ClaimTypes.Role, RoleValues.USER);
				policy.RequireRole(RoleValues.USER);
			});
		services.AddAuthentication()
			.AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters {
				IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.Key),
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true,
				ValidAudience = jwtConfiguration.ValidAudience,
				ValidIssuer = jwtConfiguration.ValidIssuer
			});
		return services;
	}
}
