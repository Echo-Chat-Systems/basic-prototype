using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Channel;
using Microsoft.EntityFrameworkCore;

namespace Server.Database.Models.Chat;

[PrimaryKey(nameof(Id))]
public class Channel
{
	public required Snowflake Id { get; init; }

	public required string Name { get; set; }
	public required int Index { get; set; }
	public required JChannelCustomisation Customisation { get; set; }
	public required JChannelConfig Config { get; set; }

	public Guild? Guild { get; set; }
	public Channel? Parent { get; set; }
	public List<ChannelMember> Members { get; set; }
}