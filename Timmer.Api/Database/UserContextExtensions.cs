namespace Timmer.Api.Database;

using Configuration;
using Constant;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

public static class UserContextExtensions {
	public static IServiceCollection AddUserContext(this IServiceCollection service, WebApplicationBuilder builder) {
		var connectionString = builder.Configuration["ConnectionStrings:MariaDb"]!;
		var userSeed = new UserSeed(builder);
		builder.Services.AddMySQLServer<UserContext>(connectionString);
		service.AddDbContext<UserContext>(optionsBuilder => {
			optionsBuilder.UseSeeding((context, _) => {
				context.Seed(userSeed);
				context.SaveChanges();
			});
			optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) => {
				context.Seed(userSeed);
				await context.SaveChangesAsync(cancellationToken);
			});
		});
		return service;
	}

	private static void Seed(this DbContext context, UserSeed userSeed) {
		var users = context.Set<User>().ToList();
		var userInfo = new User { Name = userSeed.Name, Email = userSeed.Email };
		var existedAdmin = users.Count(user =>
			(user.Role & Roles.Admin) != 0 &&
			user.Email == userInfo.Email &&
			user.Name == userInfo.Name);
		if (existedAdmin > 0) {
			return;
		}

		var passwordHasher = new PasswordHasher<User>();

		var admin = new User(userInfo) {
			Role = Roles.Admin, PasswordHash = passwordHasher.HashPassword(userInfo, userSeed.Password)
		};
		context.Set<User>().Add(admin);
	}
}
