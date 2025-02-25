namespace Timmer.Application.Domain.Authorization.Dto;

public record TokenDto(string Token, DateTime ExpiresIn);
