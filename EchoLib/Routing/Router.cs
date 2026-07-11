using EchoLib.Protocol;
using EchoLib.Routing;
using EchoLib.Routing.Discovery;
using EchoLib.Routing.Identification;
using EchoLib.Routing.Responses;
using EchoLib.Routing.Storage;
using WebSocketSharper;

namespace EchoLib.Core.Routing;

public sealed class Router
{
	private readonly RouteRegistry _routes;
	private readonly TargetInstanceRegistry _targets;
	private readonly PendingResponseRegistry _pendingResponses;

	public Router(IServiceProvider serviceProvider)
	{
		// Get registries from service provider
		_routes = new RouteRegistry();
		_targets = new TargetInstanceRegistry();
		_pendingResponses = new PendingResponseRegistry();
		
		// Find routes (cached, so it's fine to run this in non-static code)
		RouteFinder.Discover(serviceProvider, _routes);
	}

	public T GetTarget<T>() where T : ITarget
	{
		return _targets.Get<T>();
	}
	
	public async void Receive(Envelope message, WebSocket socket)
	{
		// Check if the envelope has an MID
		if (_pendingResponses.TryRemove(message.MessageId, out IPendingRequest? request))
		{
			if (message.Target == "error") request!.Fail(message);
			else request!.Complete(message);
			return;
		}
		
		RouteDescriptor? route = _routes.Get(message.Target, message.Data.Action);
		if (route == null)
			throw new InvalidOperationException($"Couldn't find route for {message.Target}:{message.Data.Action}");

		await route.Invoke(new RoutingContext
		{
			MessageId = message.MessageId,
			Socket = socket,
		}, message.Data.Parameters);
	}
	
	
}