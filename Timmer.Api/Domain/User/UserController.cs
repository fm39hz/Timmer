namespace Timmer.Api.Domain.User;

using Dto;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/[controller]")]
public class UserController(IUserService service) : ControllerBase {
	[HttpGet("")]
	public async Task<IValueHttpResult<IEnumerable<UserResponseDto>>> FindAll() {
		var users = await service.FindAll();
		var dtos = users.Select(user => new UserResponseDto(user)).ToList();
		return TypedResults.Ok(dtos);
	}

	[HttpGet("{id:guid}")]
	public async Task<IValueHttpResult<UserResponseDto>> FindOne(Guid id) {
		var user = await service.FindOne(id);
		if (user == null) {
			return TypedResults.NotFound<UserResponseDto>(null);
		}

		return TypedResults.Ok(new UserResponseDto(user));
	}

	[HttpPost("")]
	public async Task<IValueHttpResult<UserResponseDto>> Create([FromBody] UserRequestDto user) {
		var createdUser = await service.Create(user.ToModel());
		return TypedResults.Created(createdUser.Id.ToString(), new UserResponseDto(createdUser));
	}

	[HttpPut("")]
	public async Task<IValueHttpResult<UserResponseDto>> Update([FromBody] User user) =>
		TypedResults.Ok(new UserResponseDto(await service.Update(user)));
}
