namespace Timmer.Application;

using Constant;
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
		app.UseSwaggerUI();
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseMiddlewareScope();
		app.UseHttpsRedirection();
		app.Run();
	}

	private static WebApplication Build(WebApplicationBuilder builder) {
		builder.Services.AddOpenApi();
		builder.Services.AddLogging(static logging => logging.AddFilter(
				"Microsoft.EntityFrameworkCore.Database.Command",
				LogLevel.Warning)
			);
		builder.Services.AddUserContext(builder);
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddControllers();
		builder.Services.AddMvc();
		builder.Services.AddSwaggerGen(static opt => {
			opt.SwaggerDoc(
				RouteConstant.VERSION,
				new OpenApiInfo { Title = "Timmer API", Version = RouteConstant.VERSION }
				);
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
