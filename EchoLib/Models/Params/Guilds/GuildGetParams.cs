using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Params.Guilds;

public class GuildGetParams : IParam
{
	public string Action => "get";

	public required Snowflake Id { get; init; }
}