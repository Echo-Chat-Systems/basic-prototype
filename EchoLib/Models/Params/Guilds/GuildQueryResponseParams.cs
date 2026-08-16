using EchoLib.Core;
using EchoLib.Models.Data.Guild;

namespace EchoLib.Models.Params.Guilds;

public class GuildQueryResponseParams : IParam
{
	public string Action => RouteNames.Guilds.Query;

	public required IEnumerable<JGuild> Guilds { get; init; }
}