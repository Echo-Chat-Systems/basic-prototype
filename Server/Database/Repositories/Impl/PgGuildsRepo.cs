using EchoLib.Core.Snowflake;
using Server.Database.Models.Chat;

namespace Server.Database.Repositories.Impl;

public class PgGuildsRepo : IGuildsRepo
{
	public GuildDbm? Get(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public async Task<GuildDbm?> GetAsync(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public GuildDbm Insert(GuildDbm.New guild)
	{
		throw new NotImplementedException();
	}

	public async Task<GuildDbm> InsertAsync(GuildDbm.New guild)
	{
		throw new NotImplementedException();
	}

	public GuildDbm Update(GuildDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<GuildDbm> UpdateAsync(GuildDbm item)
	{
		throw new NotImplementedException();
	}

	public bool Delete(GuildDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(GuildDbm item)
	{
		throw new NotImplementedException();
	}
}