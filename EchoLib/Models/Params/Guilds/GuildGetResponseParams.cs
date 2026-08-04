using EchoLib.Core;
using EchoLib.Models.Data.Guild;

namespace EchoLib.Models.Params.Guilds;

public class GuildGetResponseParams : IParam
{
	public string Action => RouteNames.Guilds.Get;

	public required JGuild? Guild { get; init; }
}