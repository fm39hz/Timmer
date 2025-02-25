namespace Timmer.Api.Domain.User.Dto;

using Contract;
using Microsoft.OpenApi.Extensions;

public class UserResponseDto(UserModel user) : IResponseDto {
	public string Role { get; init; } = user.Role.GetDisplayName().ToUpperInvariant();
	public string Name { get; init; } = user.Name;
	public string Email { get; init; } = user.Email;
	public bool IsVerified { get; init; } = user.IsVerified;
	public Guid Id { get; init; } = user.Id;
}
