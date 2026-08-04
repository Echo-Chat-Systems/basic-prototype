using EchoLib.Core;
using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Params.Guilds;

public class GuildDeleteParams : IParam
{
	public string Action => RouteNames.Guilds.Delete;

	public required Snowflake Id { get; init; }
}