namespace Timmer.Domain.User;

using Common.Contract;

public interface IUserService : ICrudService<UserModel> {
	public Task<UserModel?> FindOne(string email, string password);
}
