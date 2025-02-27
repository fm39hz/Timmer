namespace Timmer.Data.Database;

using Domain.User;
using Domain.UserTask;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext {
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
		Database.EnsureCreated();
	}

	[UsedImplicitly] private DbSet<UserModel> Users { get; set; } = null!;
	[UsedImplicitly] private DbSet<UserTaskModel> Tasks { get; set; } = null!;
}
