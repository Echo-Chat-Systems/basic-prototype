using EchoLib.Core.Routing.Exceptions;
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
		// Lookup by target string
		_targets.Targets.TryGetValue(envelope.Target, out ITarget? target);

		// Ensure target exists
		if (target is null) throw new InvalidOperationException($"Unknown target: {envelope.Target}");

		// Cast targetType to target
		try
		{
			await target.HandleAsync(ctx, envelope);
		}
		catch (ProtocolException ex)
		{
			await ctx.SendError(envelope.FromError(ex));
		}
	}

	public T? GetTarget<T>() where T : ITarget
	{
		// Get target instance 
		return _targets.GetTarget<T>();
	}
}