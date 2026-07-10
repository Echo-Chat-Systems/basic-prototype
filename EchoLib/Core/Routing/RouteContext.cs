using WebSocketSharper;

namespace EchoLib.Core.Routing;

public class RouteContext
{
	public required Guid? MessageId { get; init; }
	public required WebSocket Socket { get; init; }
}