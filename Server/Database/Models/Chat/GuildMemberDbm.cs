using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.Guild;
using EchoLib.Models.Data.User;

namespace Server.Database.Models.Chat;

public class GuildMemberDbm : BaseDbm<GuildMemberDbm.New>
{
	public required Snowflake GuildId { get; init; }
	public required PublicSigningKey UserId { get; init; }
	public string? Nickname { get; set; }
	public required JGuildCustomisation GuildCustomisationOverride { get; set; }
	public required JProfile UserProfileOverride { get; set; }

	public class New : NewBase
	{
		public required PublicSigningKey UserId { get; init; }
		public string? Nickname { get; init; }

		public required JGuildCustomisation GuildCustomisationOverride { get; set; } = JGuildCustomisation.Empty;
		public required JProfile UserProfileOverride { get; set; } = JProfile.Empty;
	}
}