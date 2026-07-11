using WebSocketSharper;

namespace EchoLib.Routing;

public class RoutingContext
{
	public required Guid? MessageId { get; init; }
	public required WebSocket Socket { get; init; }
}