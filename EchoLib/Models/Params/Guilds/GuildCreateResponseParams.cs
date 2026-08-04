using EchoLib.Core;
using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Params.Guilds;

public class GuildCreateResponseParams : IParam
{
	public string Action => RouteNames.Guilds.Create;

	public required Snowflake Id { get; init; }
}