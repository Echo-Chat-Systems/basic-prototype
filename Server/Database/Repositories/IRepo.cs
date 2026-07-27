using Server.Database.Models;

namespace Server.Database.Repositories;

public interface IRepo<T, TId, TNew> where T : BaseDbm<TNew> where TNew : BaseDbm<TNew>.NewBase
{
	public T? Get(TId id);
	public Task<T?> GetAsync(TId id);

	public T Insert(TNew item);
	public Task<T> InsertAsync(TNew item);

	public T Update(T item);
	public Task<T> UpdateAsync(T item);

	public bool Delete(T item);
	public Task<bool> DeleteAsync(T item);
}