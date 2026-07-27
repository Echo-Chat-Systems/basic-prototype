using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data;
using EchoLib.Models.Data.Guild;

namespace Server.Database.Models.Chat;

public class GuildDbm : BaseDbm<GuildDbm.New>
{
	public required PublicSigningKey OwnerId { get; init; }
	public required string Name { get; set; }
	public required JGuildCustomisation Customisation { get; set; }
	public required JGuildConfig Config { get; set; }

	public class New : NewBase
	{
		public required PublicSigningKey OwnerId { get; init; }
		public required string Name { get; init; }

		public JGuildCustomisation Customisation { get; set; } = JGuildCustomisation.Empty;
		public JGuildConfig Config { get; set; } = JGuildConfig.Empty;
	}
}