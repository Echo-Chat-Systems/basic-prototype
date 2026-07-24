using Dapper;
using EchoLib.Crypto.Signing;
using Microsoft.Extensions.Logging;
using Npgsql;
using Server.Database.Models.Public;

namespace Server.Database.Repositories.Impl;

public class PgUsersRepo(IDbConnectionProvider connectionProvider, ILogger<PgUsersRepo> logger) : IUsersRepo
{
	private const string GetUserQuery = "SELECT * FROM public.users WHERE id = @Id";
	private const string InsertUserQuery = "INSERT INTO public.users VALUES (id = @Id, encryption_key = @Ek, username = @Username, tag = @Tag, profile = @Profile, settings = @Settings) RETURNING *;";

	public UserDbm? Get(PublicSigningKey id)
	{
		// Get data source
		using NpgsqlConnection con = connectionProvider.GetSync<NpgsqlConnection>();

		// Execute
		return con.QueryFirstOrDefault<UserDbm>(GetUserQuery, new { Id = id });
	}

	public async Task<UserDbm?> GetAsync(PublicSigningKey id)
	{
		await using NpgsqlConnection con = await connectionProvider.Get<NpgsqlConnection>();
		return await con.QueryFirstOrDefaultAsync<UserDbm>(GetUserQuery, new { Id = id });
	}

	public UserDbm Insert(UserDbm.New user)
	{
		// Get data source
		using NpgsqlConnection con = connectionProvider.GetSync<NpgsqlConnection>();

		return con.QueryFirstOrDefault<UserDbm>(InsertUserQuery, user) ?? throw new InvalidOperationException();
	}

	public async Task<UserDbm> InsertAsync(UserDbm.New user)
	{
		await using NpgsqlConnection con = await connectionProvider.Get<NpgsqlConnection>();
		return await con.QueryFirstOrDefaultAsync<UserDbm>(InsertUserQuery, user) ?? throw new InvalidOperationException();
	}
}