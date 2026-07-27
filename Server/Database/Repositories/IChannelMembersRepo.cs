using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Server.Database.Discovery;
using Server.Database.Models.Chat;
using Server.Database.Repositories.Impl;

namespace Server.Database.Repositories;

[Repo(typeof(PgChannelMembersRepo))]
public interface IChannelMembersRepo : IRepo<ChannelMemberDbm, Snowflake, ChannelMemberDbm.New>
{
	/// <summary>
	/// Queries for all users in a specific channel.
	/// </summary>
	public IEnumerable<ChannelMemberDbm> Query(Snowflake channelId);

	/// <inheritdoc cref="IChannelMembersRepo.Query(Snowflake)"/>
	public Task<IEnumerable<ChannelMemberDbm>> QueryAsync(Snowflake channelId);

	/// <summary>
	/// Queries for all channels a specific user is in.
	/// </summary>
	public IEnumerable<ChannelMemberDbm> Query(PublicSigningKey userId);

	/// <inheritdoc cref="IChannelMembersRepo.Query(PublicSigningKey)"/>
	public Task<IEnumerable<ChannelMemberDbm>> QueryAsync(PublicSigningKey userId);
}