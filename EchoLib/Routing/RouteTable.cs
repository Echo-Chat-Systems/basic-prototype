using EchoLib.Core.Routing;

namespace EchoLib.Routing;

public sealed class RouteTable
{
	public required Dictionary<(string target, string action), RouteDescriptor> Routes { get; init; }
}