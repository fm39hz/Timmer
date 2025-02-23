namespace Timmer.Api.Domain.User;

using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public sealed class UserService(UserContext context) : IUserService {
	private static PasswordHasher<User> PasswordHasher => new();
	public DbSet<User> Entities => context.Set<User>();

	public async Task<User?> FindOne(Guid id) => await Entities.FirstOrDefaultAsync(user => user.Id == id);

	public async Task<User?> FindOne(string email, string password) {
		var user = await Entities.FirstOrDefaultAsync(u => u.Email == email);

		if (user == null) {
			return null;
		}

		var isPasswordValid = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
		return isPasswordValid == PasswordVerificationResult.Failed? null : user;
	}

	public async Task<IEnumerable<User>> FindAll() => await Entities.ToListAsync();

	public async Task<User> Create(User entity) {
		var hashedPassword = PasswordHasher.HashPassword(entity, entity.PasswordHash);
		var result = Entities.Add(new User(entity) { PasswordHash = hashedPassword });
		await context.SaveChangesAsync();
		return result.Entity;
	}

	public async Task<User> Update(Guid id, User entity) {
		var hashedPassword = PasswordHasher.HashPassword(entity, entity.PasswordHash);
		var user = new User(entity) { Id = id, PasswordHash = hashedPassword };
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
