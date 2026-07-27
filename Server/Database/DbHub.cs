using Server.Database.Repositories;

namespace Server.Database;

public class DbHub(
	IUsersRepo users,
	IGuildsRepo guilds,
	IChannelsRepo channels,
	IChannelMembersRepo channelMembers,
	IGuildMembersRepo guildMembers,
	IRolesRepo rolesRepo
)
{
	public IUsersRepo Users { get; } = users;
	public IGuildsRepo Guilds { get; } = guilds;
	public IChannelsRepo Channels { get; } = channels;
	public IChannelMembersRepo ChannelMembers { get; } = channelMembers;
	public IGuildMembersRepo GuildMembers { get; } = guildMembers;
	public IRolesRepo Roles = rolesRepo;
}