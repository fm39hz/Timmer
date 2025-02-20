namespace Timmer.Api.Configuration;

public sealed record UserSeed {
	public UserSeed(WebApplicationBuilder builder) {
		Name = builder.Configuration["UserSeed:Name"]!;
		Email = builder.Configuration["UserSeed:Email"]!;
		Password = builder.Configuration["UserSeed:Password"]!;
	}

	public string Name { get; }
	public string Email { get; }
	public string Password { get; }
}
