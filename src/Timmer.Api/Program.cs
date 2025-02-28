namespace Timmer.Api;

using Domain.Constant;
using Extension;
using Infrastructure.Persistence.Database;
using Microsoft.OpenApi.Models;

public static class Program {
	public static async Task Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);
		var app = Build(builder);
		if (app.Environment.IsDevelopment()) {
			app.UseHsts();
			app.MapOpenApi();
			app.UseExceptionHandler(new ExceptionHandlerOptions {
				AllowStatusCode404Response = true, ExceptionHandlingPath = "/error"
			});
			{
				await using var scope = app.Services.CreateAsyncScope();
				await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				await dbContext.Database.EnsureCreatedAsync();
			}
		}

		app.MapControllers();
		app.UseSwagger();
		app.UseSwaggerUI(opt => {
			opt.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
		});
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseMiddlewareScope();
		app.UseHttpsRedirection();
		await app.RunAsync();
	}

	private static WebApplication Build(WebApplicationBuilder builder) {
		builder.Services.AddOpenApi();
		builder.Services.AddLogging(static logging => logging.AddFilter(
				"Microsoft.EntityFrameworkCore.Database.Command",
				LogLevel.Warning)
			);
		builder.Services.AddDatabaseContext(builder);
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
