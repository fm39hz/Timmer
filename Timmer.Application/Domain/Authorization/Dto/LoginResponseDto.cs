namespace Timmer.Application.Domain.Authorization.Dto;

public record LoginResponseDto {
	public LoginResponseDto(TokenDto accessToken, TokenDto refreshToken) {
		AccessToken = accessToken.Token;
		RefreshToken = refreshToken.Token;
		Expires = accessToken.ExpiresIn;
		RefreshExpires = refreshToken.ExpiresIn;
	}

	public string AccessToken { get; init; }
	public string RefreshToken { get; init; }
	public DateTime Expires { get; init; }
	public DateTime RefreshExpires { get; init; }
}
