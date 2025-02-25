namespace Timmer.Api.Domain.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Constant;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using User;

[ApiController]
[Route(RouteConstant.DEFAULT_ROUTE)]
public class AuthorizationController(IUserService userService, ITokenGenerator tokenGenerator) : ControllerBase {
	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IResult> Login([FromBody] LoginRequest request) {
		var user = await userService.FindOne(request.Email, request.Password);
		if (user == null) {
			return TypedResults.Unauthorized();
		}

		var accessToken = tokenGenerator.GenerateToken(user);
		var refreshToken = tokenGenerator.GenerateToken(user, true);
		var response = new LoginResponseDto(accessToken, refreshToken);
		return TypedResults.Ok(response);
	}

	[Authorize]
	[HttpPost("refresh")]
	public async Task<IResult> Refresh([FromBody] TokenDto request) {
		var claimsIdentity = User.Identity as ClaimsIdentity;
		var userId = claimsIdentity!.FindFirst(JwtRegisteredClaimNames.Sub)!.Value;
		var user = await userService.FindOne(new Guid(userId));
		if (user == null) {
			return TypedResults.Unauthorized();
		}

		var accessToken = tokenGenerator.GenerateToken(user);
		var refreshToken = tokenGenerator.GenerateToken(user, true);
		var response = new LoginResponseDto(accessToken, refreshToken);
		return TypedResults.Ok(response);
	}
}
