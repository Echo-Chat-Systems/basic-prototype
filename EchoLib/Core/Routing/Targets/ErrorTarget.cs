using EchoLib.Core.Routing.Attributes;
using EchoLib.Core.Routing.Events;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Models.Params.Generic;
using Microsoft.Extensions.Logging;

namespace EchoLib.Core.Routing.Targets;

public class ErrorTarget(EventBus bus, ILogger<ErrorTarget> logger) : TargetBase<ErrorTarget>(logger)
{
	public override string Name => "error";

	[ActionHandler("error")]
	public Task HandleError(RoutingContext ctx, ErrorParameters parameters)
	{
		Logger.LogError("Error {ErrorName} on action {Target}{Action}", parameters.Message, parameters.Source.Target, parameters.Source.Action);

		// Check if there are any registered callbacks, and if so, execute them
		ExceptionCallbackRegistry.Get(parameters.Message);
		
		// Publish error
		return bus.PublishAsync(new ProtocolErrorEvent
		{
			Message = parameters.Message,
			Source = parameters.Source
		});
	}
}

