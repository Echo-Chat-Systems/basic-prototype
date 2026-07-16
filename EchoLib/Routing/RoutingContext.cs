using System.Text.Json;
using EchoLib.Protocol;
using EchoLib.Transport;
using WebSocketSharper;

namespace EchoLib.Routing;

public class RoutingContext
{
	public required Guid? MessageId { get; init; }
	public required Envelope<JsonElement> OriginalMessage { get; init; }
	public required WebSocket Socket { get; init; }
	public required IMessageEndpoint Endpoint { get; init; }

	public async Task ReplyAsync(object parameters)
	{
		// Ensure message ID isn't null
		if (MessageId is null) throw new InvalidOperationException("Cannot respond to message without message ID");

		// Construct and send a response
		await Endpoint.SendAsync(OriginalMessage.Target, OriginalMessage.Data.Action, parameters, MessageId);
	}
}