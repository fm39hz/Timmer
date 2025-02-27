namespace Timmer.Infrastructure.Persistence.Repository;

using Database;
using Domain.Common.Repository;
using Domain.Entity.User;
using Microsoft.EntityFrameworkCore;

public sealed class UserRepository(ApplicationDbContext context) : CrudRepository<UserModel>(context), IUserRepository {
	public async Task<UserModel?> FindOneByEmail(string email) =>
		await Entities.FirstOrDefaultAsync(u => u.Email == email);
}
