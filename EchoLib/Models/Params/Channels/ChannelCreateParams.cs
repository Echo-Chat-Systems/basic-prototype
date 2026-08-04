using EchoLib.Core;

namespace EchoLib.Models.Params.Channels;

public class ChannelCreateParams : IParam
{
	public string Action => RouteNames.Channels.Create;

	public required string Name { get; init; }
}