namespace Timmer.Api.Extension;

using JetBrains.Annotations;
using Middleware;

public static class MiddlewareExtension {
	[UsedImplicitly]
	public static IApplicationBuilder UseMiddlewareScope(this IApplicationBuilder builder) {
		builder.UseMiddleware<UserValidationMiddleware>();
		return builder;
	}
}
