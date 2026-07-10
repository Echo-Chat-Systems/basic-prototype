using EchoLib.Core.Routing.Events;
using EchoLib.Models.Params.Generic;
using Microsoft.Extensions.Logging;

namespace EchoLib.Core.Routing.Targets;

public class ErrorTarget(EventBus bus, ILogger<ErrorTarget> logger) : TargetBase<ErrorTarget>(logger)
{
	public override string Name => "error";

	[ActionHandler("error")]
	public Task HandleError(RoutingContext ctx, ErrorParameters parameters)
	{
		Logger.LogError("Error {ErrorName} on action {Action}", parameters.Message, parameters.Action);

		// Publish error
		return bus.PublishAsync(new ProtocolErrorEvent
		{
			Message = parameters.Message,
			Source = parameters.Source
		});
	}
}

