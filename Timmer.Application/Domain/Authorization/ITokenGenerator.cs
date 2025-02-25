namespace Timmer.Application.Domain.Authorization;

using Dto;
using User;

public interface ITokenGenerator {
	public TokenDto GenerateToken(UserModel user, bool isRefreshToken = false);
}
