namespace Timmer.Api.Domain.User;

using Common.Constant;
using Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Timmer.Domain.User;

[ApiController]
[Route(RouteConstant.CONTROLLER)]
public sealed class UserController(IUserService service) : ControllerBase, IUserController {
	[HttpGet("{id:guid}")]
	[Authorize(RoleConstant.USER)]
	public async Task<IValueHttpResult<IUserResponseDto>> FindOne(Guid id) {
		var user = await service.FindOne(id);
		return user == null? TypedResults.NotFound<UserResponseDto>(null) : TypedResults.Ok(new UserResponseDto(user));
	}

	[HttpGet("")]
	[Authorize(RoleConstant.ADMIN)]
	public async Task<IValueHttpResult<IEnumerable<IUserResponseDto>>> FindAll() {
		var users = await service.FindAll();
		var dtos = users.Select(user => new UserResponseDto(user)).ToList();
		return TypedResults.Ok(dtos);
	}

	[HttpPost("")]
	[Authorize(RoleConstant.ADMIN)]
	public async Task<IValueHttpResult<IUserResponseDto>> Create([FromBody] IUserRequestDto entity) {
		var createdUser = await service.Create(entity.ToModel());
		return TypedResults.Created(createdUser.Id.ToString(), new UserResponseDto(createdUser));
	}

	[HttpPut("{id:guid}")]
	[Authorize(RoleConstant.USER)]
	public async Task<IValueHttpResult<IUserResponseDto>> Update(Guid id, [FromBody] IUserRequestDto entity) =>
		TypedResults.Ok(new UserResponseDto(await service.Update(id, entity.ToModel())));

	[HttpDelete("{id:guid}")]
	[Authorize(RoleConstant.ADMIN)]
	public async Task<IValueHttpResult<int>> Delete(Guid id) => TypedResults.Ok(await service.Delete(id));
}
