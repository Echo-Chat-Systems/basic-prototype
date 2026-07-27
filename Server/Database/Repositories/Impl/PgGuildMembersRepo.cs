using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Server.Database.Models.Chat;

namespace Server.Database.Repositories.Impl;

public class PgGuildMembersRepo : IGuildMembersRepo
{
	public GuildMemberDbm? Get(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public Task<GuildMemberDbm?> GetAsync(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public GuildMemberDbm Insert(GuildMemberDbm.New item)
	{
		throw new NotImplementedException();
	}

	public Task<GuildMemberDbm> InsertAsync(GuildMemberDbm.New item)
	{
		throw new NotImplementedException();
	}

	public GuildMemberDbm Update(GuildMemberDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<GuildMemberDbm> UpdateAsync(GuildMemberDbm item)
	{
		throw new NotImplementedException();
	}

	public bool Delete(GuildMemberDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(GuildMemberDbm item)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<GuildMemberDbm> Query(Snowflake guildId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<GuildMemberDbm>> QueryAsync(Snowflake guildId)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<GuildMemberDbm> Query(PublicSigningKey userId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<GuildMemberDbm>> QueryAsync(PublicSigningKey userId)
	{
		throw new NotImplementedException();
	}
}