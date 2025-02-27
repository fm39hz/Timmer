namespace Timmer.Application.Dto.Authorization;

public record TokenDto(string Token, DateTime ExpiresIn);
