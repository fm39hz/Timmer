namespace Timmer.Api.Domain.Base;

using Infrastructure.User;
using Timmer.Domain.User;

public static class ServiceBuilder {
	public static IServiceCollection AddServices(this IServiceCollection service) {
		service.AddScoped<IUserRepository, UserRepository>();
		service.AddScoped<IUserService, UserService>();
		return service;
	}
}
