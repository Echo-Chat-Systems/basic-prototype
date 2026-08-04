using EchoLib.Core;
using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Params.Guilds;

public class GuildGetParams : IParam
{
	public string Action => RouteNames.Guilds.Get;

	public required Snowflake Id { get; init; }
}