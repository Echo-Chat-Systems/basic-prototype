using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Server.Database.Models.Chat;

namespace Server.Database.Repositories.Impl;

public class PgRolesRepo : IRolesRepo
{
	public RoleDbm? Get(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public Task<RoleDbm?> GetAsync(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public RoleDbm Insert(RoleDbm.New item)
	{
		throw new NotImplementedException();
	}

	public Task<RoleDbm> InsertAsync(RoleDbm.New item)
	{
		throw new NotImplementedException();
	}

	public RoleDbm Update(RoleDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<RoleDbm> UpdateAsync(RoleDbm item)
	{
		throw new NotImplementedException();
	}

	public bool Delete(RoleDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(RoleDbm item)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<RoleDbm> Query(Snowflake guildId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<RoleDbm>> QueryAsync(Snowflake guildId)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<RoleDbm> Query(PublicSigningKey userId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<RoleDbm>> QueryAsync(PublicSigningKey userId)
	{
		throw new NotImplementedException();
	}
}