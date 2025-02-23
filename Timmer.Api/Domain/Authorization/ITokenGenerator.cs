namespace Timmer.Api.Domain.Authorization;

using Dto;
using User;

public interface ITokenGenerator {
	public TokenDto GenerateToken(User user, bool isRefreshToken = false);
}
