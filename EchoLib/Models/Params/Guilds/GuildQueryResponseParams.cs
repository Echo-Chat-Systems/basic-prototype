using EchoLib.Models.Data.Guild;

namespace EchoLib.Models.Params.Guilds;

public class GuildQueryResponseParams : IParam
{
	public string Action => "query";

	public required IEnumerable<JGuild> Guilds { get; init; }
}