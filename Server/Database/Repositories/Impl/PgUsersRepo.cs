using Dapper;
using EchoLib.Core.Crypto.Signing;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NpgsqlTypes;
using Server.Database.Models.Public;

namespace Server.Database.Repositories.Impl;

public class PgUsersRepo(IServiceProvider services) : PgBaseRepo(services), IUsersRepo
{
	public UserDbm? Get(UserId id)
	{
		// Get data source
		NpgsqlDataSource src = Services.GetRequiredService<NpgsqlDataSource>();

		// Create connection 
		using NpgsqlConnection con = src.OpenConnection();

		// Create command
		CommandDefinition command = new(
			"SELECT * FROM public.users WHERE id = @id", new
			{
				id
			}
		);

		// Execute
		return con.QueryFirstOrDefault<UserDbm>(command);
	}

	public async Task<UserDbm> GetAsync(UserId id)
	{
		throw new NotImplementedException();
	}
}