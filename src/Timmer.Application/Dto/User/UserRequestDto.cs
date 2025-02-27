namespace Timmer.Application.Dto.User;

using Domain.Constant;
using Domain.Entity.User;

public record UserRequestDto(string Role, string Name, string Email, string Password)
	: IRequestDto<UserModel> {
	public UserModel ToModel() => new() { Name = Name, Email = Email, Role = Role.ToRole(), PasswordHash = Password };
}
