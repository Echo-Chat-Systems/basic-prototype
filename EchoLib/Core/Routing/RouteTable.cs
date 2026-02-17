namespace EchoLib.Core.Routing;

public sealed class RouteTable
{
	private readonly Dictionary<(string target, string action), RouteDescriptor> _routes = new();

	public void Register<THandler>(string target, string action)
	{
		_routes[(target, action)] = new RouteDescriptor(typeof(THandler));
	}

	public void RegisterBulk<THandler>(string target, IList<string> actions)
	{
		foreach (string action in actions) Register<THandler>(target, action);
	}

	public bool TryResolve(string target, string action, out RouteDescriptor? descriptor)
	{
		return _routes.TryGetValue((target, action), out descriptor);
	}
}

public record RouteDescriptor(Type HandlerType);