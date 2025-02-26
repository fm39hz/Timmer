namespace Timmer.Application.Domain.User;

using Database;
using Microsoft.EntityFrameworkCore;

public sealed class UserRepository(DatabaseContext context) : CrudRepository<UserModel>(context), IUserRepository {
	public async Task<UserModel?> FindUseWithEmail(string email) =>
		await Entities.FirstOrDefaultAsync(u => u.Email == email);
}
