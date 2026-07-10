using EchoLib.Core.Routing;

namespace EchoLib.Routing;

public sealed class RouteDescriptor
{
	public required string Target { get; init; }

	public required string Action { get; init; }

	public required Type RequestType { get; init; }

	public required Type ResponseType { get; init; }

	public required Func<object, RouteContext, object, Task<object?>> Handler { get; init; }
}