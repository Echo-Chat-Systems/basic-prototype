using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Server.Database.Discovery;
using Server.Database.Models;
using Server.Database.Models.Public;

namespace Server.Database.Repositories.Impl;

public abstract class BaseRepo<TData, TId, TNew>(ILogger logger, IDbConnectionProvider connectionProvider)  where TData : BaseDbm<TNew> where TNew : BaseDbm<TNew>.NewBase
{
	protected abstract string GetQuery { get; }
	protected abstract string InsertQuery { get; }
	protected abstract string UpdateQuery { get; }
	protected abstract string DeleteQuery { get; }

	protected TData? Get(TId id)
	{
		logger.LogDb(nameof(Get), id);
		using NpgsqlConnection con = connectionProvider.GetSync<NpgsqlConnection>();
		return con.QueryFirstOrDefault<TData>(GetQuery, new { Id = id });
	}

	protected async Task<TData?> GetAsync(TId id)
	{
		logger.LogDb(nameof(GetAsync), id);
		await using NpgsqlConnection con = await connectionProvider.Get<NpgsqlConnection>();
		return await con.QueryFirstOrDefaultAsync<TData>(GetQuery, new { Id = id });
	}

	protected TData Insert(TNew item)
	{
		logger.LogDb(nameof(Insert), item);
		using NpgsqlConnection con = connectionProvider.GetSync<NpgsqlConnection>();
		return con.QueryFirstOrDefault<TData>(InsertQuery, item) ?? throw new InvalidOperationException();
	}

	protected async Task<TData> InsertAsync(TNew item)
	{
		logger.LogDb(nameof(InsertAsync), item);
		await using NpgsqlConnection con = await connectionProvider.Get<NpgsqlConnection>();
		return await con.QueryFirstOrDefaultAsync<TData>(InsertQuery, item) ?? throw new InvalidOperationException();
	}

	protected TData Update(TData item)
	{
		logger.LogDb(nameof(Update), item);
		using NpgsqlConnection con = connectionProvider.GetSync<NpgsqlConnection>();
		return con.QueryFirstOrDefault<TData>(UpdateQuery, item) ?? throw new InvalidOperationException();
	}

	protected async Task<TData> UpdateAsync(TData item)
	{
		logger.LogDb(nameof(UpdateAsync), item);
		await using NpgsqlConnection con = await connectionProvider.Get<NpgsqlConnection>();
		return await con.QueryFirstOrDefaultAsync<TData>(UpdateQuery, item) ?? throw new InvalidOperationException();
	}

	protected void Delete(TId id)
	{
		logger.LogDb(nameof(Delete), id);
		using NpgsqlConnection con = connectionProvider.GetSync<NpgsqlConnection>();
		con.Execute(DeleteQuery, id);
	}

	protected async Task DeleteAsync(TId id)
	{
		logger.LogDb(nameof(DeleteAsync), id);
		await using NpgsqlConnection con = await connectionProvider.Get<NpgsqlConnection>();
		await con.ExecuteAsync(DeleteQuery, id);
	}

	protected IEnumerable<TData> Many(string name, string command, object param)
	{
		logger.LogDb(name, param);
		using NpgsqlConnection con = connectionProvider.GetSync<NpgsqlConnection>();
		return con.Query<TData>(command, param);
	}

	protected async Task<IEnumerable<TData>> ManyAsync(string name, string command, object param)
	{
		logger.LogDb(name, param);
		await using NpgsqlConnection con = await connectionProvider.Get<NpgsqlConnection>();
		return await con.QueryAsync<TData>(command, param);
	}
}