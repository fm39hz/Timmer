namespace Timmer.Api.Domain.User;

using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public sealed class UserService(UserContext context) : IUserService {
	private static PasswordHasher<UserModel> PasswordHasher => new();
	public DbSet<UserModel> Entities => context.Set<UserModel>();

	public async Task<UserModel?> FindOne(Guid id) => await Entities.FirstOrDefaultAsync(user => user.Id == id);

	public async Task<UserModel?> FindOne(string email, string password) {
		var user = await Entities.FirstOrDefaultAsync(u => u.Email == email);

		if (user == null) {
			return null;
		}

		var isPasswordValid = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
		return isPasswordValid == PasswordVerificationResult.Failed ? null : user;
	}

	public async Task<IEnumerable<UserModel>> FindAll() => await Entities.ToListAsync();

	public async Task<UserModel> Create(UserModel entity) {
		var hashedPassword = PasswordHasher.HashPassword(entity, entity.PasswordHash);
		var result = Entities.Add(new UserModel(entity) { PasswordHash = hashedPassword });
		await context.SaveChangesAsync();
		return result.Entity;
	}

	public async Task<UserModel> Update(Guid id, UserModel entity) {
		var hashedPassword = PasswordHasher.HashPassword(entity, entity.PasswordHash);
		var user = new UserModel(entity) { Id = id, PasswordHash = hashedPassword };
		var result = Entities.Update(user);
		await context.SaveChangesAsync();
		return result.Entity;
	}

	public async Task<int> Delete(Guid id) {
		try {
			var entity = await FindOne(id);
			if (entity != null) {
				Entities.Remove(entity);
			}

			return await context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException e) {
			Console.WriteLine(e);
			throw;
		}
	}
}
