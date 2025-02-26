namespace Timmer.Application.Domain.User;

using Database;
using Microsoft.EntityFrameworkCore;

public sealed class UserRepository(ApplicationDbContext context) : CrudRepository<UserModel>(context), IUserRepository {
	public async Task<UserModel?> FindOneByEmail(string email) =>
		await Entities.FirstOrDefaultAsync(u => u.Email == email);
}
