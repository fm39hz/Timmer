namespace Timmer.Infrastructure.User;

using Common.Repository;
using Data.Database;
using Domain.User;
using Microsoft.EntityFrameworkCore;

public sealed class UserRepository(ApplicationDbContext context) : CrudRepository<UserModel>(context), IUserRepository {
	public async Task<UserModel?> FindOneByEmail(string email) =>
		await Entities.FirstOrDefaultAsync(u => u.Email == email);
}
