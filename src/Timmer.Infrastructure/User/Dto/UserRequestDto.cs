namespace Timmer.Infrastructure.User.Dto;

using Common.Constant;
using Domain.User;

public record UserRequestDto(string Role, string Name, string Email, string Password)
	: IUserRequestDto {
	public UserModel ToModel() => new() { Name = Name, Email = Email, Role = Role.ToRole(), PasswordHash = Password };
}
