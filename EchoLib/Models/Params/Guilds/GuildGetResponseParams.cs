using EchoLib.Models.Data.Guild;

namespace EchoLib.Models.Params.Guilds;

public class GuildGetResponseParams : IParam
{
	public string Action => "get";

	public required JGuild? Guild { get; init; }
}