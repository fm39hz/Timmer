namespace Timmer.Api.Controller.User;

using Application.Dto.User;
using Application.Service.Contract;
using Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(RouteConstant.CONTROLLER)]
[Authorize(RoleConstant.USER)]
public sealed class UserController(IUserService service) : ControllerBase, IUserController {
	[HttpGet("{id:guid}")]
	public async Task<IValueHttpResult<UserResponseDto>> FindOne(Guid id) {
		var user = await service.FindOne(id);
		return user == null ? TypedResults.NotFound<UserResponseDto>(null) : TypedResults.Ok(new UserResponseDto(user));
	}

	[HttpPut("{id:guid}")]
	public async Task<IValueHttpResult<UserResponseDto>> Update(Guid id, [FromBody] UserRequestDto entity) =>
		TypedResults.Ok(new UserResponseDto(await service.Update(id, entity.ToModel())));
}
