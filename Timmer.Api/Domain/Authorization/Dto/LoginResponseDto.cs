namespace Timmer.Api.Domain.Authorization.Dto;

public record LoginResponseDto(string AccessToken, string RefreshToken, Guid UserId, int ExpiresIn);
