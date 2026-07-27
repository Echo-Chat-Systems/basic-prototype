using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.User;

namespace EchoLib.Models.Data.Guild;

public class JGuildMember
{
	public required Snowflake Id { get; init; }
	public required PublicSigningKey UserId { get; init; }
	public required IEnumerable<Snowflake> Roles { get; set; }
	public string? Nickname { get; set; }
	public JProfile? UserOverride { get; set; }
	public JProfile? GuildOverride { get; set; }
}