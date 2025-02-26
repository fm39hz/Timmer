namespace Timmer.Application.Domain.Base;

using User;

public static class ServiceBuilder {
	public static IServiceCollection AddServices(this IServiceCollection service) {
		service.AddScoped<IUserRepository, UserRepository>();
		service.AddScoped<IUserService, UserService>();
		return service;
	}
}
