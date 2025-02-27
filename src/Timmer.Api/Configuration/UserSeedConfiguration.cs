namespace Timmer.Api.Configuration;

public sealed record UserSeedConfiguration {
	public UserSeedConfiguration(IConfiguration configuration) {
		Name = configuration["UserSeed:Name"]!;
		Email = configuration["UserSeed:Email"]!;
		Password = configuration["UserSeed:Password"]!;
	}

	public string Name { get; }
	public string Email { get; }
	public string Password { get; }
}
