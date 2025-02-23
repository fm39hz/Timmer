namespace Timmer.Api;

using Database;
using Domain.Authorization;
using Domain.Base;
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
		app.UseSwagger();
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseLoggerMiddleware();
		app.UseHttpsRedirection();
		app.Run();
	}

	private static WebApplication Build(WebApplicationBuilder builder) {
		builder.Services.AddOpenApi();
		builder.Services.AddUserContext(builder);
		builder.Services.AddLogging(logging =>
			logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning));
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddControllers();
		builder.Services.AddSwaggerGen();
		builder.Services.AddServices();
		builder.Services.AddJwt(builder.Configuration);
		return builder.Build();
	}
}
