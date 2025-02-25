namespace Timmer.Application.Domain.User;

using Contract;

public interface IUserService : ICrudService<UserModel> {
	public Task<UserModel?> FindOne(string email, string password);
}
