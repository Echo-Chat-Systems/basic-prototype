using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Protocol.Models.Params.Generic;
using EchoLib.Routing.Responses;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebSocketSharper;

namespace EchoLib.Transport;

public class WebsocketEndpoint(WebSocket sock, IServiceProvider services) : IMessageEndpoint
{
	private readonly PendingResponseRegistry _pendingResponses = services.GetRequiredService<PendingResponseRegistry>();

	public async Task ErrorAsync(ProtocolException err, Guid mid)
	{
		// Build message envelope
		Envelope<ErrorParameters> exEnvelope = new()
		{
			MessageId = mid,
			Target = "error",
			Data = new ActionWrapper<ErrorParameters>
			{
				Action = "error",
				Parameters = new ErrorParameters
				{
					Message = err.Message
				}
			}
		};

		await sock.SendTaskAsync(JsonConvert.SerializeObject(exEnvelope));
	}

	public async Task SendAsync<T>(string target, string action, T param, Guid mid)
	{
		// Build message envelope
		Envelope<T> envelope = new()
		{
			MessageId = mid,
			Target = target,
			Data = new ActionWrapper<T>
			{
				Action = action,
				Parameters = param
			}
		};

		// Serialise and send this
		await sock.SendTaskAsync(JsonConvert.SerializeObject(envelope));
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
		sock.SendTaskAsync(JsonConvert.SerializeObject(envelope));

		// Register in pending responses registry
		return _pendingResponses.Register<TResponse>(mid);
	}
}