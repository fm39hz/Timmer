namespace Timmer.Api;

using Database;
using Domain.Authorization;
using Domain.Base;
using Microsoft.OpenApi.Models;
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
		app.UseSwaggerUI(opt => opt.SwaggerEndpoint("v1/swagger.json", "Timmer API v1"));
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
		builder.Services.AddMvc();
		builder.Services.AddSwaggerGen(opt => {
			opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Timmer API", Version = "v1" });
			opt.AddSecurityDefinition("bearerAuth",
				new OpenApiSecurityScheme {
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Name = "Authorization",
					Description = "JWT Authorization header using the Bearer scheme."
				});
			opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
				{
					new OpenApiSecurityScheme {
						Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
					},
					Array.Empty<string>()
				}
			});
		});
		builder.Services.AddServices();
		builder.Services.AddJwt(builder.Configuration);
		return builder.Build();
	}
}
