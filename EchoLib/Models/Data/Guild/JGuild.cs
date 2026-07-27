using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.Channel;

namespace EchoLib.Models.Data.Guild;

public class JGuild
{
	public required Snowflake Id { get; init; }
	public required string Name { get; init; }
	public required PublicSigningKey Owner { get; set; }
	public required IEnumerable<JGuildMember> Members { get; set; }
	public required IEnumerable<JChannel> Channels { get; set; }

}