namespace Timmer.Application.Domain.User;

using Contract;

public interface IUserRepository : IRepository<UserModel> {
	public Task<UserModel?> FindOneByEmail(string email);
}
