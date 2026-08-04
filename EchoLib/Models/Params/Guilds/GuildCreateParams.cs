using EchoLib.Core;

namespace EchoLib.Models.Params.Guilds;

public class GuildCreateParams : IParam
{
	public string Action => RouteNames.Guilds.Create;

	public required string Name { get; init; }
}