using EchoLib.Core.Snowflake;
using Server.Database.Models.Chat;

namespace Server.Database.Repositories.Impl;

public class PgChannelsRepo : IChannelsRepo
{
	public ChannelDbm? Get(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public Task<ChannelDbm?> GetAsync(Snowflake id)
	{
		throw new NotImplementedException();
	}

	public ChannelDbm Insert(ChannelDbm.New item)
	{
		throw new NotImplementedException();
	}

	public Task<ChannelDbm> InsertAsync(ChannelDbm.New item)
	{
		throw new NotImplementedException();
	}

	public ChannelDbm Update(ChannelDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<ChannelDbm> UpdateAsync(ChannelDbm item)
	{
		throw new NotImplementedException();
	}

	public bool Delete(ChannelDbm item)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAsync(ChannelDbm item)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<ChannelDbm> Query(Snowflake guildId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<ChannelDbm>> QueryAsync(Snowflake guildId)
	{
		throw new NotImplementedException();
	}
}