namespace Timmer.Application.Middleware;

public static class MiddlewareExtensions {
	public static IApplicationBuilder UseMiddlewareScope(this IApplicationBuilder builder) {
		builder.UseMiddleware<UserValidationMiddleware>();
		return builder;
	}
}
