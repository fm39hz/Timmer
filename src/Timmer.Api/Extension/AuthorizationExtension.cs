namespace Timmer.Api.Extension;

using System.Security.Claims;
using Application.Service.Contract;
using Application.Service.Implementation;
using Domain.Constant;
using Infrastructure.Configuration;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

public static class AuthorizationExtension {
	[UsedImplicitly]
	public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration) {
		var jwtConfiguration = new JwtConfiguration(configuration);
		services.AddSingleton<ITokenGenerator, TokenGenerator>();
		services.AddAuthorizationBuilder()
			.AddPolicy(RoleConstant.ADMIN, policy => {
				policy.RequireClaim(ClaimTypes.Role, RoleConstant.ADMIN);
				policy.RequireRole(RoleConstant.ADMIN);
			})
			.AddPolicy(RoleConstant.USER, policy => {
				policy.RequireClaim(ClaimTypes.Role, RoleConstant.USER, RoleConstant.ADMIN);
				policy.RequireRole(RoleConstant.USER, RoleConstant.ADMIN);
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
