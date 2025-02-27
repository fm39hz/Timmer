namespace Timmer.Common.Repository;

using Microsoft.EntityFrameworkCore;
using Model;

public abstract class CrudRepository<T>(DbContext context) : IRepository<T> where T : BaseModel, IModel {
	protected DbSet<T> Entities => context.Set<T>();
	public async Task<T?> FindOne(Guid id) => await Entities.FirstOrDefaultAsync(entity => entity.Id == id);

	public async Task<IEnumerable<T>> FindAll() => await Entities.ToListAsync();

	public async Task<T> Create(T entity) {
		var result = Entities.Add(entity);
		await context.SaveChangesAsync();
		return result.Entity;
	}

	public async Task<T> Update(T entity) {
		var result = Entities.Update(entity);
		await context.SaveChangesAsync();
		return result.Entity;
	}

	public async Task<int> Delete(Guid id) {
		try {
			var entity = await FindOne(id);
			if (entity != null) {
				Entities.Remove(entity);
			}

			return await context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException e) {
			Console.WriteLine(e);
			throw;
		}
	}
}
