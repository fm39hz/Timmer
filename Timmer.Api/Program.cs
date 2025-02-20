namespace Timmer.Api;

using Configuration;
using Database;
using Domain.User;
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
		var userSeed = new UserSeed(builder);
		var connectionString = builder.Configuration["ConnectionStrings:MariaDb"]!;
		builder.Services.AddOpenApi();
		builder.Services.AddMySQLServer<UserContext>(connectionString, optionsBuilder => {
			optionsBuilder.MigrationsAssembly(typeof(Program).Assembly.FullName);
		});
		builder.Services.AddUserContext(userSeed);
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
