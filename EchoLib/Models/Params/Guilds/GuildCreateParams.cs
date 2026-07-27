namespace EchoLib.Models.Params.Guilds;

public class GuildCreateParams : IParam
{
	public string Action => "create";

	public required string Name { get; init; }
}