namespace Timmer.Api.Database;

using Domain.User;
using Microsoft.EntityFrameworkCore;

public sealed class UserContext : DbContext {
	public UserContext(DbContextOptions<UserContext> options) : base(options) {
		Database.EnsureCreated();
	}

	private DbSet<User> Users { get; set; } = null!;
}
