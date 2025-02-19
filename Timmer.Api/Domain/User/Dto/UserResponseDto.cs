namespace Timmer.Api.Domain.User.Dto;

using Contract;
using Microsoft.OpenApi.Extensions;

public class UserResponseDto(User user) : IResponseDto {
	public string Role { get; init; } = user.Role.GetDisplayName().ToUpperInvariant();
	public string Name { get; init; } = user.Name;
	public string Email { get; init; } = user.Email;
	public bool EmailConfirmed { get; init; } = user.EmailConfirmed;
	public Guid Id { get; init; } = user.Id;
}
