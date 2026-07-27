using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Channel;

namespace Server.Database.Models.Chat;

public class ChannelDbm : BaseDbm<ChannelDbm.New>
{
	public Snowflake? GuildId { get; init; }
	public required string Name { get; set; }
	public Snowflake? Parent { get; set; }
	public required int Index { get; set; }
	public required JChannelCustomisation Customisation { get; set; }
	public required JChannelConfig Config { get; set; }

	public class New : NewBase
	{
		public Snowflake? GuildId { get; set; } = null;
		public required string Name { get; init; }
		public Snowflake? Parent { get; init; }
		public required int Index { get; init; }
		public JChannelCustomisation Customisation { get; set; } = JChannelCustomisation.Empty;
		public JChannelConfig Config { get; set; } = JChannelConfig.Empty;
	}
}