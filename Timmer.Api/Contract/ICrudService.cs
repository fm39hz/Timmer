namespace Timmer.Api.Contract;

using Microsoft.EntityFrameworkCore;

public interface ICrudService<T> where T : class {
	public DbSet<T> Entities { get; }
	public Task<T?> FindOne(Guid id);
	public Task<IEnumerable<T>> FindAll();
	public Task<T> Create(T entity);
	public Task<T> Update(T entity);
	public Task<int> Delete(Guid id);
}
