using System.Text.Json;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Params;
using EchoLib.Protocol;
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
	public required IServiceProvider Services { get; init; }

	/// <summary>
	/// Allows preprocessor to pass user to method.
	/// </summary>
	public PublicSigningKey? User { get; set; }

	public async Task ReplyAsync<T>(T parameters) where T : IParam
	{
		// Construct and send a response
		await Endpoint.SendAsync(OriginalMessage.Target, parameters, MessageId);
	}
}