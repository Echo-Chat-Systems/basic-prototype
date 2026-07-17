using System.Text.Json;
using EchoLib.Protocol;
using EchoLib.Protocol.Models.Params;
using EchoLib.Transport;
using Newtonsoft.Json.Linq;
using WebSocketSharper;

namespace EchoLib.Routing;

public class RoutingContext
{
	public required Guid MessageId { get; init; }
	public required Envelope<JToken> OriginalMessage { get; init; }
	public required WebSocket Socket { get; init; }
	public required IMessageEndpoint Endpoint { get; init; }

	public async Task ReplyAsync<T>(T parameters) where T : IParam
	{
		// Construct and send a response
		await Endpoint.SendAsync(OriginalMessage.Target, parameters, MessageId);
	}
}