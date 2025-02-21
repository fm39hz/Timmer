namespace Timmer.Api.Middleware;

using Contract;
using Domain.Base;

public class CrudLoggerMiddleware(RequestDelegate next) {
	private static readonly Action<ILogger, string, Exception?> _dRequestReceived =
		LoggerMessage.Define<string>(LogLevel.Information, new EventId(0, nameof(RequestReceived)),
			"Request received: {Item}");

	public async Task InvokeAsync(
		HttpContext context,
		ILogger<ICrudController<IModel, IResponseDto, IRequestDto<IModel>>> logger) {
		var message =
			$"{context.GetEndpoint()?.DisplayName} responded with status code {context.Response.StatusCode}";
		RequestReceived(logger, message);
		await next(context);
	}

	public void RequestReceived(ILogger logger, string message) => _dRequestReceived.Invoke(logger, message, null);
}

public static class LoggerMiddlewareExtensions {
	public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder) =>
		builder.UseMiddleware<CrudLoggerMiddleware>();
}
