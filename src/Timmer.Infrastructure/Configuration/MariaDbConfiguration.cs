namespace Timmer.Infrastructure.Configuration;

using Microsoft.Extensions.Configuration;

public sealed record MariaDbConfiguration {
	public MariaDbConfiguration(IConfiguration configuration) {
		ConnectionString = configuration["ConnectionStrings:MariaDb"]!;
	}

	public string ConnectionString { get; } = string.Empty;
}
