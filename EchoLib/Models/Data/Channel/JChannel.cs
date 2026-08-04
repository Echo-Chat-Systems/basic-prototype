using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Data.Channel;

public class JChannel
{
	public Snowflake Id { get; init; }
	public Snowflake? GuildId { get; init; }
	public required string Name { get; set; }
	public Snowflake? Parent { get; set; }
	public int? Index { get; set; }
	public IEnumerable<JChannelMember>? Members { get; set; }
	public required JChannelCustomisation Customisation { get; set; }
	public required JChannelConfig Config { get; set; }
}