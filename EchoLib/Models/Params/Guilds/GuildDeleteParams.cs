using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Params.Guilds;

public class GuildDeleteParams : IParam
{
	public string Action => "delete";

	public required Snowflake Id { get; init; }
}