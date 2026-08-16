using EchoLib.Core;
using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Params.Channels;

public class ChannelGetParams : IParam
{
	public string Action => RouteNames.Channels.Get;
	
	public required Snowflake Id { get; init; }
}