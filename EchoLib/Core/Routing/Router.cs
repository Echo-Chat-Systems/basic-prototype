using EchoLib.Core.Routing.Exceptions;
using WebSocketSharper;
using WebSocketSharper.Server;

namespace EchoLib.Core.Routing;

public interface IMessageRouter
{
	Task RouteAsync(RoutingContext ctx, MessageEnvelope<object> envelope);
}

public sealed class Router : IMessageRouter
{
	public async Task RouteAsync(RoutingContext ctx, MessageEnvelope<object> envelope)
	{
		// Lookup by target string
		TargetRegistry.Targets.TryGetValue(envelope.Target, out ITarget? target);

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
		return TargetRegistry.GetTarget<T>();
	}
}