namespace Timmer.Domain.User;

using Common.Repository;

public interface IUserRepository : IRepository<UserModel> {
	public Task<UserModel?> FindOneByEmail(string email);
}
