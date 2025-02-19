namespace Timmer.Api.Database;

using Domain.User;
using Microsoft.EntityFrameworkCore;

public sealed class DatabaseContext : DbContext {
	public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
		Database.EnsureCreated();
	}

	private DbSet<User> Users { get; set; } = null!;
}
