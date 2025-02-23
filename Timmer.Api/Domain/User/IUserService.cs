namespace Timmer.Api.Domain.User;

using Contract;

public interface IUserService : ICrudService<User> {
	public Task<User?> FindOne(string email, string password);
}
