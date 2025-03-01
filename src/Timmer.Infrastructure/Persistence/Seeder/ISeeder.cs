namespace Timmer.Infrastructure.Persistence.Seeder;

using Microsoft.EntityFrameworkCore;

public interface ISeeder {
    bool SeedData(DbContext context);
    Task<bool> SeedDataAsync(DbContext context, CancellationToken cancellationToken = default);
}
