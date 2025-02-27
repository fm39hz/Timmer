namespace Timmer.Application.Service.Contract;

using Domain.Entity.User;
using Dto.Authorization;

public interface ITokenGenerator {
	public TokenDto GenerateToken(UserModel user, bool isRefreshToken = false);
}
