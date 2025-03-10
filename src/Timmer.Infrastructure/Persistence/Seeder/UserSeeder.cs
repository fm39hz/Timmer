namespace Timmer.Infrastructure.Persistence.Seeder;

using DataGenerator.User;
using DataGenerator.UserTask;
using Domain.Entity.User;
using Domain.Entity.UserTask;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserSeeder : ISeeder {
	private const int TARGET_SEED_NUMBER = 50;

	public bool SeedData(DbContext context) {
		var userCount = context.Set<UserModel>().Count();
		if (userCount >= TARGET_SEED_NUMBER) {
			return false;
		}

		CreateUsers(context, userCount);
		return true;
	}

	public async Task<bool> SeedDataAsync(DbContext context, CancellationToken cancellationToken = default) {
		var userCount = await context.Set<UserModel>().CountAsync(cancellationToken);
		if (userCount >= TARGET_SEED_NUMBER) {
			return false;
		}

		CreateUsers(context, userCount);
		return true;
	}

	private static void CreateUsers(DbContext context, int existingUserCount) {
		var passwordHasher = new PasswordHasher<UserModel>();
		var users = context.Set<UserModel>();

		for (var i = 0; i < TARGET_SEED_NUMBER - existingUserCount; i++) {
			var userInfo = UserDataGenerator.Generate();
			var user = new UserModel(userInfo) {
				PasswordHash = passwordHasher.HashPassword(userInfo, userInfo.PasswordHash)
			};

			users.Add(user);
			context.Set<UserTaskModel>().Add(UserTaskDataGenerator.Generate(user));
		}
	}
}
