namespace Timmer.Api.Extension;

using Common.Constant;
using Configuration;
using Data.Database;
using Infrastructure.User;
using Infrastructure.UserTask;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Timmer.Domain.User;
using Timmer.Domain.UserTask;

public static class UserContextExtensions {
	[UsedImplicitly]
	public static IServiceCollection AddUserContext(this IServiceCollection service, WebApplicationBuilder builder) {
		var mariaDbConfiguration = new MariaDbConfiguration(builder.Configuration);
		var userSeed = new UserSeedConfiguration(builder.Configuration);
		builder.Services.AddMySQLServer<ApplicationDbContext>(mariaDbConfiguration.ConnectionString);
		service.AddDbContext<ApplicationDbContext>(optionsBuilder => {
			optionsBuilder.UseSeeding((context, _) => {
				Seed(context, userSeed);
				context.SaveChanges();
			});
			optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) => {
				Seed(context, userSeed);
				await context.SaveChangesAsync(cancellationToken);
			});
		});
		return service;
	}

	private static void Seed(this DbContext context, UserSeedConfiguration userSeedConfiguration) {
		var users = context.Set<UserModel>().ToList();
		var adminIndo = new UserModel { Name = userSeedConfiguration.Name, Email = userSeedConfiguration.Email };
		var existedAdmin = users.Count(user =>
			(user.Role & Roles.Admin) != 0 &&
			user.Email == adminIndo.Email &&
			user.Name == adminIndo.Name);
		if (existedAdmin > 0) {
			return;
		}

		var passwordHasher = new PasswordHasher<UserModel>();

		var admin = new UserModel(adminIndo) {
			Role = Roles.Admin, PasswordHash = passwordHasher.HashPassword(adminIndo, userSeedConfiguration.Password)
		};
		context.Set<UserTaskModel>().Add(new UserTaskModel {
			User = admin, Name = "Test Task", Description = "Declare Task"
		});
		context.Set<UserModel>().Add(admin);
		var userCount = users.Count;
		if (userCount >= 50) {
			return;
		}

		for (var i = 0; i < 50 - userCount; i++) {
			var userInfo = UserDataGenerator.Generate();
			var user = new UserModel(userInfo) {
				PasswordHash = passwordHasher.HashPassword(userInfo, userInfo.PasswordHash)
			};
			context.Set<UserTaskModel>().Add(UserTaskDataGenerator.Generate(user));
			context.Set<UserModel>().Add(user);
		}
	}
}
