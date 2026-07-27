using Server.Database.Repositories;

namespace Server.Database;

public class DbHub
(
	IUsersRepo users,
	IGuildsRepo guilds,
	IChannelsRepo channels,
	IChannelMembersRepo channelMembers
)
{
	public IUsersRepo Users { get; } = users;
	public IGuildsRepo Guilds { get; } = guilds;
	public IChannelsRepo Channels { get; } = channels;
	public IChannelMembersRepo ChannelMembers { get; } = channelMembers;
}