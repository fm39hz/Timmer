namespace Timmer.Api.Domain.User;

using Constant;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize(RoleConstant.ADMIN)]
[Route(RouteConstant.CONTROLLER)]
public sealed class UserController(IUserService service) : ControllerBase, IUserController {
	[HttpGet("{id:guid}")]
	public async Task<IValueHttpResult<UserResponseDto>> FindOne(Guid id) {
		var user = await service.FindOne(id);
		return user == null ? TypedResults.NotFound<UserResponseDto>(null) : TypedResults.Ok(new UserResponseDto(user));
	}

	[HttpGet("")]
	public async Task<IValueHttpResult<IEnumerable<UserResponseDto>>> FindAll() {
		var users = await service.FindAll();
		var dtos = users.Select(user => new UserResponseDto(user)).ToList();
		return TypedResults.Ok(dtos);
	}

	[HttpPost("")]
	public async Task<IValueHttpResult<UserResponseDto>> Create([FromBody] UserRequestDto entity) {
		var createdUser = await service.Create(entity.ToModel());
		return TypedResults.Created(createdUser.Id.ToString(), new UserResponseDto(createdUser));
	}

	[HttpPut("{id:guid}")]
	public async Task<IValueHttpResult<UserResponseDto>> Update(Guid id, [FromBody] UserRequestDto entity) =>
		TypedResults.Ok(new UserResponseDto(await service.Update(id, entity.ToModel())));

	[HttpDelete("{id:guid}")]
	public async Task<IValueHttpResult<int>> Delete(Guid id) => TypedResults.Ok(await service.Delete(id));
}
