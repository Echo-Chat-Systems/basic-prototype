using EchoLib.Core;
using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Params.Channels;

public class ChannelCreateResponseParams : IParam
{
	public string Action => RouteNames.Channels.Create;

	public required Snowflake Id { get; init; }
}