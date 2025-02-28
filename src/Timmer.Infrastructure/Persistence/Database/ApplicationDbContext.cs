namespace Timmer.Infrastructure.Persistence.Database;

using Domain.Entity.User;
using Domain.Entity.UserTask;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options) {
	[UsedImplicitly] private DbSet<UserModel> Users { get; set; } = null!;
	[UsedImplicitly] private DbSet<UserTaskModel> Tasks { get; set; } = null!;
}
