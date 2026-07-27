using Core.Models.Permissions;
using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Guild;

namespace Server.Database.Models.Chat;

public class RoleDbm : BaseDbm<RoleDbm.New>
{
	public required Snowflake GuildId { get; init; }
	public required string Name { get; set; }
	public required JRoleCustomisation Customisation { get; set; }
	public required GuildPermissions GuildPermissions { get; set; }
	public required TextChannelPermissions TextPermissions { get; set; }
	public required VoiceChannelPermissions VoicePermissions { get; set; }

	// Property for DTO Mapper
	public JRolePermissionSet Permissions => new()
	{
		Guild = GuildPermissions,
		Text = TextPermissions,
		Voice = VoicePermissions
	};

	public class New : NewBase
	{
		public required Snowflake GuildId { get; init; }
		public required string Name { get; init; }
		public required JRoleCustomisation Customisation { get; set; } = JRoleCustomisation.Empty;
		public required GuildPermissions GuildPermissions { get; set; }
		public required TextChannelPermissions TextPermissions { get; set; }
		public required VoiceChannelPermissions VoicePermissions { get; set; }
	}
}