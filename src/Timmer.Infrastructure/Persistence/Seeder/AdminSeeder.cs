namespace Timmer.Infrastructure.Persistence.Seeder;

using Configuration;
using DataGenerator.UserTask;
using Domain.Constant;
using Domain.Entity.User;
using Domain.Entity.UserTask;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AdminSeeder(UserSeedConfiguration userSeedConfiguration) : ISeeder {
	public bool SeedData(DbContext context) {
		if (CheckAdminExists(context)) {
			return false;
		}

		CreateAdmin(context);
		return true;
	}

	public async Task<bool> SeedDataAsync(DbContext context, CancellationToken cancellationToken = default) {
		if (await CheckAdminExistsAsync(context, cancellationToken)) {
			return false;
		}

		CreateAdmin(context);
		return true;
	}

	private bool CheckAdminExists(DbContext context) {
		var users = context.Set<UserModel>();
		var adminInfo = new UserModel { Name = userSeedConfiguration.Name, Email = userSeedConfiguration.Email };

		return users.Any(user =>
			(user.Role & Roles.Admin) != 0 &&
			user.Email == adminInfo.Email &&
			user.Name == adminInfo.Name);
	}

	private async Task<bool> CheckAdminExistsAsync(DbContext context, CancellationToken cancellationToken) {
		var users = context.Set<UserModel>();
		var adminInfo = new UserModel { Name = userSeedConfiguration.Name, Email = userSeedConfiguration.Email };

		return await users.AnyAsync(user =>
				(user.Role & Roles.Admin) != 0 &&
				user.Email == adminInfo.Email &&
				user.Name == adminInfo.Name,
			cancellationToken);
	}

	private void CreateAdmin(DbContext context) {
		var users = context.Set<UserModel>();
		var adminInfo = new UserModel { Name = userSeedConfiguration.Name, Email = userSeedConfiguration.Email };
		var passwordHasher = new PasswordHasher<UserModel>();

		var admin = new UserModel(adminInfo) {
			Role = Roles.Admin,
			PasswordHash = passwordHasher.HashPassword(adminInfo, userSeedConfiguration.Password)
		};

		context.Set<UserTaskModel>().Add(UserTaskDataGenerator.Generate(admin));
		users.Add(admin);
	}
}
