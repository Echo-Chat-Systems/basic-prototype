using EchoLib.Crypto.Signing;
using Server.Database.Models;

namespace Server.Database.Repositories;

public interface IRepo<T, in TId, in TNew> where T : BaseDbm<TNew> where TNew : BaseDbm<TNew>.NewBase
{
	public T? Get(TId id);
	public Task<T?> GetAsync(TId id);

	public T Insert(TNew item);
	public Task<T> InsertAsync(TNew item);

	public T Update(T item);
	public Task<T> UpdateAsync(T item);

	public void Delete(TId item);
	public Task DeleteAsync(TId item);
}