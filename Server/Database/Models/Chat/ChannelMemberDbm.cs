using Core.Models.Permissions;
using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;

namespace Server.Database.Models.Chat;

public class ChannelMemberDbm : BaseDbm<ChannelMemberDbm.New>
{
	public required PublicSigningKey UserId { get; init; }
	public required Snowflake ChannelId { get; init; }
	public required TextChannelPermissions Permissions { get; init; }

	public class New : NewBase
	{
		public required PublicSigningKey UserId { get; init; }
		public required Snowflake ChannelId { get; init; }
		public required TextChannelPermissions Permissions { get; init; }
	}
}