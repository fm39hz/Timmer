namespace Timmer.Domain.Common.Repository;

using Entity.User;

public interface IUserRepository : IRepository<UserModel> {
	public Task<UserModel?> FindOneByEmail(string email);
}
