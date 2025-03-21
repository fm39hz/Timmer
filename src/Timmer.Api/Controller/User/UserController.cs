namespace Timmer.Api.Controller.User;

using Application.Dto.User;
using Application.Service.Contract;
using Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(RouteConstant.CONTROLLER)]
public sealed class UserController(IUserService service) : ControllerBase, IUserController {
	[HttpGet("{id:guid}")]
	[Authorize(RoleConstant.USER)]
	public async Task<IValueHttpResult<UserResponseDto>> FindOne(Guid id) {
		var user = await service.FindOne(id);
		return user == null ? TypedResults.NotFound<UserResponseDto>(null) : TypedResults.Ok(new UserResponseDto(user));
	}

	[HttpGet("")]
	[Authorize(RoleConstant.ADMIN)]
	public async Task<IValueHttpResult<IEnumerable<UserResponseDto>>> FindAll() {
		var users = await service.FindAll();
		var dtos = users.Select(user => new UserResponseDto(user)).ToList();
		return TypedResults.Ok(dtos);
	}

	[HttpPost("")]
	[Authorize(RoleConstant.ADMIN)]
	public async Task<IValueHttpResult<UserResponseDto>> Create([FromBody] UserRequestDto entity) {
		var createdUser = await service.Create(entity.ToModel());
		return TypedResults.Created(createdUser.Id.ToString(), new UserResponseDto(createdUser));
	}

	[HttpPut("{id:guid}")]
	[Authorize(RoleConstant.USER)]
	public async Task<IValueHttpResult<UserResponseDto>> Update(Guid id, [FromBody] UserRequestDto entity) =>
		TypedResults.Ok(new UserResponseDto(await service.Update(id, entity.ToModel())));

	[HttpDelete("{id:guid}")]
	[Authorize(RoleConstant.ADMIN)]
	public async Task<IValueHttpResult<int>> Delete(Guid id) => TypedResults.Ok(await service.Delete(id));
}
