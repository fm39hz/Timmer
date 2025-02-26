namespace Timmer.Application.Database;

using Configuration;
using Constant;
using Domain.User;
using Domain.UserTask;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using Utils;

public sealed class ApplicationDbContext : DbContext {
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
		Database.EnsureCreated();
	}

	[UsedImplicitly] private DbSet<UserModel> Users { get; set; } = null!;
	[UsedImplicitly] private DbSet<UserTaskModel> Tasks { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		foreach (var entity in modelBuilder.Model.GetEntityTypes()) {
			var typeName = entity.ClrType.Name;
			if (typeName.EndsWith("Model")) {
				typeName = typeName[..^5];
			}

			var tableName = Converter.ToSnakeCase(typeName) + "s";
			entity.SetTableName(tableName);

			foreach (var property in entity.GetProperties()) {
				var columnName = Converter.ToSnakeCase(property.Name);
				property.SetColumnName(columnName);
			}
		}
	}
}

public static class UserContextExtensions {
	public static IServiceCollection AddUserContext(this IServiceCollection service, WebApplicationBuilder builder) {
		var mariaDbConfiguration = new MariaDbConfiguration(builder.Configuration);
		var userSeed = new UserSeedConfiguration(builder.Configuration);
		builder.Services.AddMySQLServer<ApplicationDbContext>(mariaDbConfiguration.ConnectionString);
		service.AddDbContext<ApplicationDbContext>(optionsBuilder => {
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
