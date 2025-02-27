namespace Timmer.Api.Configuration;

public sealed record JwtConfiguration {
	public JwtConfiguration(IConfiguration configuration) {
		Key = configuration["Jwt:Secret"]!.Select(Convert.ToByte).ToArray();
		ValidAudience = configuration["Jwt:Audience"]!;
		ValidIssuer = configuration["Jwt:Issuer"]!;
	}

	public byte[] Key { get; }
	public string ValidAudience { get; }
	public string ValidIssuer { get; }
}
