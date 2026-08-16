using EchoLib.Core;
using EchoLib.Models.Data.Channel;

namespace EchoLib.Models.Params.Channels;

public class ChannelGetResponseParams : IParam
{
	public string Action => RouteNames.Channels.Get;
	
	public JChannel? Channel { get; init; }
}