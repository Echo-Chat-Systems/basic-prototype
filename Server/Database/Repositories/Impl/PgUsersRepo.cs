using Dapper;
using EchoLib.Crypto.Signing;
using Microsoft.Extensions.Logging;
using Npgsql;
using Server.Database.Discovery;
using Server.Database.Models.Public;

namespace Server.Database.Repositories.Impl;

public class PgUsersRepo(IDbConnectionProvider connectionProvider, ILogger<PgUsersRepo> logger) : IUsersRepo
{
	private const string GetUserQuery = @"SELECT * FROM public.users WHERE id = @Id";
	private const string InsertUserQuery = @"INSERT INTO public.users VALUES (@Id, default, @Ek, @Username, @Tag, @Profile, @Settings, @LastOnline, @IsOnlne, @IsBanned) RETURNING *;";
	private const string UpdateUserQuery = @"";  // TODO: Update user query

	public UserDbm? Get(PublicSigningKey id)
	{
		logger.LogDb(nameof(Get), id);

		// Get data source
		using NpgsqlConnection con = connectionProvider.GetSync<NpgsqlConnection>();

		// Execute
		return con.QueryFirstOrDefault<UserDbm>(GetUserQuery, new { Id = id });
	}

	public async Task<UserDbm?> GetAsync(PublicSigningKey id)
	{
		logger.LogDb(nameof(GetAsync), id);
		await using NpgsqlConnection con = await connectionProvider.Get<NpgsqlConnection>();
		return await con.QueryFirstOrDefaultAsync<UserDbm>(GetUserQuery, new { Id = id });
	}

	public UserDbm Insert(UserDbm.New user)
	{
		logger.LogDb(nameof(Insert), user);
		// Get data source
		using NpgsqlConnection con = connectionProvider.GetSync<NpgsqlConnection>();

		return con.QueryFirstOrDefault<UserDbm>(InsertUserQuery, user) ?? throw new InvalidOperationException();
	}

	public async Task<UserDbm> InsertAsync(UserDbm.New user)
	{
		logger.LogDb(nameof(InsertAsync), user);
		await using NpgsqlConnection con = await connectionProvider.Get<NpgsqlConnection>();
		return await con.QueryFirstOrDefaultAsync<UserDbm>(InsertUserQuery, user) ?? throw new InvalidOperationException();
	}

	public UserDbm Update(UserDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<UserDbm> UpdateAsync(UserDbm item)
	{
		throw new NotImplementedException();
	}

	public bool Delete(UserDbm item)
	{
		logger.LogError("Cannot delete users! Ban a user to remove their access.");
		return false;
	}

	public Task<bool> DeleteAsync(UserDbm item)
	{
		logger.LogError("Cannot delete users! Ban a user to remove their access.");
		return Task.FromResult(false);
	}
}