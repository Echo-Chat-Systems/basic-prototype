using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Protocol.Models.Params.Generic;
using EchoLib.Routing.Responses;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebSocketSharper;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EchoLib.Transport;

public class WebsocketEndpoint(WebSocket sock, IServiceProvider services) : IMessageEndpoint
{
	private readonly PendingResponseRegistry _pendingResponses = services.GetRequiredService<PendingResponseRegistry>();

	public Task ErrorAsync(ProtocolException err, Guid? mid = null)
	{
		throw new NotImplementedException();
	}

	public async Task SendAsync<T>(string target, string action, T param, Guid? mid = null)
	{
		// Build message envelope
		Envelope<T> envelope = new()
		{
			MessageId = mid,
			Target = target,
			Data = new ActionWrapper<T>()
			{
				Action = action,
				Parameters = param
			}
		};

		// Serialise and send this
		await sock.SendTaskAsync(JsonSerializer.Serialize(envelope));
	}

	public Task<TResponse> RequestAsync<TResponse, TParam>(string target, string action, TParam param)
	{
		// Build message envelope
		Guid mid = Guid.NewGuid();
		Envelope<TParam> envelope = new()
		{
			MessageId = mid,
			Target = target,
			Data = new ActionWrapper<TParam>
			{
				Action = action,
				Parameters = param
			}
		};

		// Send request
		sock.SendTaskAsync(JsonSerializer.Serialize(envelope));

		// Register in pending responses registry
		return _pendingResponses.Register<TResponse>(mid);
	}
}