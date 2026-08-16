using EchoLib.Core;
using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Params.Channels;

public class ChannelCreateParams : IParam
{
	public string Action => RouteNames.Channels.Create;

	public required string Name { get; init; }
	public required Snowflake Guild { get; init; }
}