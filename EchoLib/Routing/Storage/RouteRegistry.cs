using EchoLib.Routing.Identification;

namespace EchoLib.Routing.Storage;

public sealed class RouteRegistry
{
	private Dictionary<(string target, string action), RouteDescriptor> _routes = new();

	public void RegisterRoute(RouteDescriptor route)
	{
		_routes[(route.Target, route.Action)] = route;
	}

	public RouteDescriptor? Get(string target, string action)
	{
		return _routes.GetValueOrDefault((target, action));
	}
}