namespace Timmer.Api.Domain.User.Dto;

using Constant;
using Contract;

public record UserRequestDto(string Role, string Name, string Email, string Password)
	: IRequestDto<User> {
	public User ToModel() => new() { Name = Name, Email = Email, Role = Role.ToRole(), PasswordHash = Password };
}
