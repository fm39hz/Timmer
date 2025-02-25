namespace Timmer.Application.Configuration;

public sealed record MariaDbConfiguration {
	public MariaDbConfiguration(IConfiguration configuration) {
		ConnectionString = configuration["ConnectionStrings:MariaDb"]!;
	}

	public string ConnectionString { get; } = string.Empty;
}
