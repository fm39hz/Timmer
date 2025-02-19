namespace Timmer.Api;

using Constant;
using Database;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using MySql.EntityFrameworkCore.Extensions;

public static class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);
		var app = Build(builder);
		if (app.Environment.IsDevelopment()) {
			app.UseHsts();
			app.MapOpenApi();
			app.UseExceptionHandler(new ExceptionHandlerOptions {
				AllowStatusCode404Response = true, ExceptionHandlingPath = "/error"
			});
		}

		app.MapControllers();
		app.UseHttpsRedirection();
		app.Run();
	}

	private static WebApplication Build(WebApplicationBuilder builder) {
		var userSeed = builder.Configuration.GetSection("UserSeed");
		var connectionString = builder.Configuration.GetConnectionString("MariaDb")!;
		builder.Services.AddOpenApi();
		builder.Services.AddMySQLServer<DatabaseContext>(connectionString, optionsBuilder => {
			optionsBuilder.MigrationsAssembly(typeof(Program).Assembly.FullName);
		});
		builder.Services.AddDbContext<DatabaseContext>(optionsBuilder => {
			optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
			optionsBuilder.UseSeeding((context, _) => {
				var users = context.Set<User>().ToList();
				var userInfo = new User {
					Name = userSeed.GetSection("Name").Value!, Email = userSeed.GetSection("Email").Value!
				};
				var existedAdmin = users.Count(user =>
					(user.Role & Roles.Admin) != 0 && user.Email == userInfo.Email && user.Name == userInfo.Name);
				var passwordHasher = new PasswordHasher<User>();
				if (existedAdmin > 0) {
					return;
				}

				var admin = new User {
					Name = userInfo.Name,
					Email = userInfo.Email,
					Role = Roles.Admin,
					PasswordHash = passwordHasher.HashPassword(userInfo, userSeed.GetSection("Password").Value!)
				};
				context.Set<User>().Add(admin);
				context.SaveChanges();
			});
		});
		builder.Services.AddLogging();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddControllers();
		builder.Services.AddSwaggerGen();
		builder.Services.AddScoped<IUserService, UserService>();
		builder.Services.AddAuthentication()
			.AddJwtBearer("some-scheme", jwtOptions => {
				jwtOptions.Authority = builder.Configuration["Api:Authority"];
				jwtOptions.Audience = builder.Configuration["Api:Audience"];
				jwtOptions.TokenValidationParameters = new TokenValidationParameters {
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidAudiences = builder.Configuration.GetSection("Api:ValidAudiences").Get<string[]>(),
					ValidIssuers = builder.Configuration.GetSection("Api:ValidIssuers").Get<string[]>()
				};
				jwtOptions.MapInboundClaims = false;
			});
		return builder.Build();
	}
}
