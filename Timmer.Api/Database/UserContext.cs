namespace Timmer.Api.Database;

using Configuration;
using Constant;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

public sealed class UserContext : DbContext {
	public UserContext(DbContextOptions<UserContext> options) : base(options) {
		Database.EnsureCreated();
	}

	private DbSet<User> Users { get; set; } = null!;
}

public static class UserContextExtensions {
	public static IServiceCollection AddUserContext(this IServiceCollection service, WebApplicationBuilder builder) {
		var mariaDbConfiguration = new MariaDbConfiguration(builder.Configuration);
		var userSeed = new UserSeedConfiguration(builder.Configuration);
		builder.Services.AddMySQLServer<UserContext>(mariaDbConfiguration.ConnectionString);
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

	private static void Seed(this DbContext context, UserSeedConfiguration userSeedConfiguration) {
		var users = context.Set<User>().ToList();
		var userInfo = new User { Name = userSeedConfiguration.Name, Email = userSeedConfiguration.Email };
		var existedAdmin = users.Count(user =>
			(user.Role & Roles.Admin) != 0 &&
			user.Email == userInfo.Email &&
			user.Name == userInfo.Name);
		if (existedAdmin > 0) {
			return;
		}

		var passwordHasher = new PasswordHasher<User>();

		var admin = new User(userInfo) {
			Role = Roles.Admin, PasswordHash = passwordHasher.HashPassword(userInfo, userSeedConfiguration.Password)
		};
		context.Set<User>().Add(admin);
	}
}
