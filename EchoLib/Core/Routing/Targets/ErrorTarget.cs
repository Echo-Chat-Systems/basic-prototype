using EchoLib.Core.Routing.Events;
using EchoLib.Models.Params.Generic;

namespace EchoLib.Core.Routing.Targets;

public class ErrorTarget(EventBus bus) : TargetBase
{
	public override string Name => "error";

	[ActionHandler("error")]
	public Task HandleError(RoutingContext ctx, ErrorParameters parameters)
	{
		// Publish error
		return bus.PublishAsync(new ProtocolErrorEvent
		{
			Message = parameters.Message,
			Source = parameters.Source
		});
	}
}

