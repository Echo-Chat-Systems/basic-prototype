using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Server.Database.Models.Chat;

namespace Server.Database.Repositories.Impl;

public class PgChannelMembersRepo : IChannelMembersRepo
{
	public ChannelMemberDbm? Get(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public Task<ChannelMemberDbm?> GetAsync(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public ChannelMemberDbm Insert(ChannelMemberDbm.New item)
	{
		throw new NotImplementedException();
	}

	public Task<ChannelMemberDbm> InsertAsync(ChannelMemberDbm.New item)
	{
		throw new NotImplementedException();
	}

	public ChannelMemberDbm Update(ChannelMemberDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<ChannelMemberDbm> UpdateAsync(ChannelMemberDbm item)
	{
		throw new NotImplementedException();
	}

	public bool Delete(ChannelMemberDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(ChannelMemberDbm item)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<ChannelMemberDbm> Query(Snowflake channelId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<ChannelMemberDbm>> QueryAsync(Snowflake channelId)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<ChannelMemberDbm> Query(PublicSigningKey userId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<ChannelMemberDbm>> QueryAsync(PublicSigningKey userId)
	{
		throw new NotImplementedException();
	}
}