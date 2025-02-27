namespace Timmer.Api.Extension;

using Infrastructure.User;
using JetBrains.Annotations;
using Timmer.Domain.User;

public static class ServiceExtension {
	[UsedImplicitly]
	public static IServiceCollection AddServices(this IServiceCollection service) {
		service.AddScoped<IUserRepository, UserRepository>();
		service.AddScoped<IUserService, UserService>();
		return service;
	}
}
