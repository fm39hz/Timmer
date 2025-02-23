namespace Timmer.Api.Domain.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using User;

[ApiController]
[Route("/api/v1/[controller]")]
public class AuthorizationController(IUserService userService, ITokenGenerator tokenGenerator) : ControllerBase {
	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IResult> Login(LoginRequest request) {
		var user = await userService.FindOne(request.Email, request.Password);
		if (user == null) {
			return TypedResults.Unauthorized();
		}

		var token = tokenGenerator.GenerateToken(user);
		return TypedResults.Ok(token);
	}
}
