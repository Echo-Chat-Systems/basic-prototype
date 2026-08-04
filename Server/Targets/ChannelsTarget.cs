using EchoLib.Core;
using EchoLib.Models.Params.Channels;
using EchoLib.Routing;
using EchoLib.Routing.Identification;
using Microsoft.Extensions.Logging;
using Server.Database;
using Server.Targets.PreProcessors;

namespace Server.Targets;

public class ChannelsTarget(ILogger<ChannelsTarget> logger, EchoContext db) : TargetBase<ChannelsTarget>(logger)
{
	public override string Name => "channels";

	[Route(RouteNames.Channels.Create)]
	[Authenticated]
	public async Task HandleCreate(RoutingContext ctx, ChannelCreateParams para)
	{
		
	}
}