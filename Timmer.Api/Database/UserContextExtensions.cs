namespace Timmer.Api.Database;

using Configuration;
using Constant;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;

public static class UserContextExtensions {
	public static IServiceCollection AddUserContext(this IServiceCollection service, UserSeed userSeed) {
		service.AddDbContext<UserContext>(optionsBuilder => {
			optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
			optionsBuilder.UseSeeding((context, _) => {
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
				context.SaveChanges();
			});
		});
		return service;
	}
}
