namespace Timmer.Application.Domain.User;

using System.Security.Claims;
using Constant;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize(RoleConstant.ADMIN)]
[Route(RouteConstant.CONTROLLER)]
public sealed class UserController(IUserService service) : ControllerBase, IUserController {
	[HttpGet("{id:guid}")]
	[Authorize(RoleConstant.USER)]
	public async Task<IValueHttpResult<UserResponseDto>> FindOne(Guid id) {
		if (!ValidateScope(id)) {
			return TypedResults.BadRequest<UserResponseDto>(null);
		}

		var user = await service.FindOne(id);
		return user == null? TypedResults.NotFound<UserResponseDto>(null) : TypedResults.Ok(new UserResponseDto(user));
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
	[Authorize(RoleConstant.USER)]
	public async Task<IValueHttpResult<UserResponseDto>> Update(Guid id, [FromBody] UserRequestDto entity) {
		if (!ValidateScope(id)) {
			return TypedResults.BadRequest<UserResponseDto>(null);
		}

		return TypedResults.Ok(new UserResponseDto(await service.Update(id, entity.ToModel())));
	}

	[HttpDelete("{id:guid}")]
	public async Task<IValueHttpResult<int>> Delete(Guid id) {
		if (!ValidateScope(id)) {
			return TypedResults.BadRequest(-1);
		}

		return TypedResults.Ok(await service.Delete(id));
	}

	private bool ValidateScope(Guid id) {
		var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
		var role = User.FindFirst(ClaimTypes.Role)!.Value;
		return userId == id || role == RoleConstant.ADMIN;
	}
}
