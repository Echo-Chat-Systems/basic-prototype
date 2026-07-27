using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Params.Guilds;

public class GuildCreateResponseParams : IParam
{
	public string Action => "create";

	public required Snowflake Id { get; init; }
}