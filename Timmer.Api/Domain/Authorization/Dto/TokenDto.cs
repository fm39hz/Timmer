namespace Timmer.Api.Domain.Authorization.Dto;

public record TokenDto(string Token, DateTime? ExpiresIn);
