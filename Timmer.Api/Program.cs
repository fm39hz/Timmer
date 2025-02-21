namespace Timmer.Api;

using Database;
using Domain.User;
using Microsoft.IdentityModel.Tokens;
using Middleware;

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
		app.UseLoggerMiddleware();
		app.UseHttpsRedirection();
		app.Run();
	}

	private static WebApplication Build(WebApplicationBuilder builder) {
		builder.Services.AddOpenApi();
		builder.Services.AddUserContext(builder);
		builder.Services.AddLogging(logging => {
			logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
		});
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
