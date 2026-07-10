using EchoLib.Core.Routing.Exceptions;
using EchoLib.Core.Routing.Registries;
using EchoLib.Core.Routing.Targets;
using EchoLib.Models.Params.Generic;
using Microsoft.Extensions.Logging;
using WebSocketSharper;
using WebSocketSharper.Server;

namespace EchoLib.Core.Routing;

public interface IMessageRouter
{
	Task RouteAsync(RoutingContext ctx, MessageEnvelope<object> envelope);
}

public sealed class Router(IServiceProvider services, ILogger<Router> logger) : IMessageRouter
{
	
	private IServiceProvider _services = services;
	private TargetRegistry _targets { get; init; } = new(services);
	
	public async Task RouteAsync(RoutingContext ctx, MessageEnvelope<object> envelope)
	{
		// Handle error target special routing first
		if (envelope.Target == "error")
		{
			// Parse envelope as error and route

		}

		// Lookup by target string
		_targets.Targets.TryGetValue(envelope.Target, out ITarget? target);

		// Ensure target exists
		if (target is null)
		{
			await ctx.SendError(envelope.FromError(new InvalidTargetException()));
			return;
		}

		// Cast targetType to target
		try
		{
			await target.HandleAsync(ctx, envelope);
		}
		catch (ProtocolException ex)
		{
			await ctx.SendError(envelope.FromError(ex));
		}
		catch (Exception e)
		{
			logger.LogError("Unhandled exception: [{ErrorType}]\n{Message}\n{StackTrace}", e.GetType().Name, e.Message, e.StackTrace);
		}
	}

	public T? GetTarget<T>() where T : ITarget
	{
		// Get target instance 
		return _targets.GetTarget<T>();
	}
}