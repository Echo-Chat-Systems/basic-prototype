using EchoLib.Core.Snowflake;
using Server.Database.Discovery;
using Server.Database.Models.Chat;
using Server.Database.Repositories.Impl;

namespace Server.Database.Repositories;

[Repo(typeof(PgChannelsRepo))]
public interface IChannelsRepo : IRepo<ChannelDbm, Snowflake, ChannelDbm.New>
{
	/// <summary>
	/// Queries for channels that belong to a specific guild.
	/// </summary>
	public IEnumerable<ChannelDbm> Query(Snowflake guildId);
	/// <inheritdoc cref="IChannelsRepo.Query"/>
	public Task<IEnumerable<ChannelDbm>> QueryAsync(Snowflake guildId);
}