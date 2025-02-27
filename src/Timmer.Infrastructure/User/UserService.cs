namespace Timmer.Infrastructure.User;

using Domain.User;
using Microsoft.AspNetCore.Identity;

public sealed class UserService(IUserRepository repository) : IUserService {
	private static PasswordHasher<UserModel> PasswordHasher => new();

	public async Task<UserModel?> FindOne(Guid id) => await repository.FindOne(id);

	public async Task<UserModel?> FindOne(string email, string password) {
		var user = await repository.FindOneByEmail(email);

		if (user == null) {
			return null;
		}

		var isPasswordValid = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
		return isPasswordValid == PasswordVerificationResult.Failed? null : user;
	}

	public async Task<IEnumerable<UserModel>> FindAll() => await repository.FindAll();

	public async Task<UserModel> Create(UserModel entity) {
		var hashedPassword = PasswordHasher.HashPassword(entity, entity.PasswordHash);
		var result = await repository.Create(new UserModel(entity) { PasswordHash = hashedPassword });
		return result;
	}

	public async Task<UserModel> Update(Guid id, UserModel entity) {
		var hashedPassword = PasswordHasher.HashPassword(entity, entity.PasswordHash);
		var user = new UserModel(entity) { Id = id, PasswordHash = hashedPassword };
		var result = await repository.Update(user);
		return result;
	}

	public async Task<int> Delete(Guid id) => await repository.Delete(id);
}
