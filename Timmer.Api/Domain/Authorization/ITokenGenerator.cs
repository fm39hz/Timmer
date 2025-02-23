namespace Timmer.Api.Domain.Authorization;

using User;

public interface ITokenGenerator {
	public string GenerateToken(User user);
}
