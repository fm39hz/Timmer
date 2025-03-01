namespace Timmer.Infrastructure.Persistence.Seeder;

using Microsoft.EntityFrameworkCore;

public class DatabaseSeeder(IEnumerable<ISeeder> seeders) {
	public void SeedAll(DbContext context) {
		foreach (var seeder in seeders) {
			seeder.SeedData(context);
		}

		context.SaveChanges();
	}

	public async Task SeedAllAsync(DbContext context, CancellationToken cancellationToken = default) {
		foreach (var seeder in seeders) {
			await seeder.SeedDataAsync(context, cancellationToken);
		}

		await context.SaveChangesAsync(cancellationToken);
	}
}
