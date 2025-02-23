namespace Timmer.Api.Domain.Authorization;

using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using User;

[ApiController]
[Route("/api/v1/[controller]")]
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
}
