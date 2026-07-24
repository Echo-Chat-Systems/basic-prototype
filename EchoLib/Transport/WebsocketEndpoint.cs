using EchoLib.Models.Params;
using EchoLib.Models.Params.Generic;
using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing.Responses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebSocketSharper;

namespace EchoLib.Transport;

public class WebsocketEndpoint(ILogger<WebsocketEndpoint> logger, WebSocket sock, PendingResponseRegistry pending) : IMessageEndpoint
{
	public WebsocketEndpoint(WebSocket sock, IServiceProvider services) : this(
		services.GetRequiredService<ILogger<WebsocketEndpoint>>(),
		sock,
		services.GetRequiredService<PendingResponseRegistry>()
	)
	{
	}

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

		logger.LogError("[{Mid}] [error:error] [{Error}] sending", mid, err.Message);

		await sock.SendTaskAsync(JsonConvert.SerializeObject(exEnvelope));
	}

	public async Task SendAsync<T>(string target, T param, Guid mid) where T : IParam
	{
		// Build message envelope
		Envelope<T> envelope = new()
		{
			MessageId = mid,
			Target = target,
			Data = new ActionWrapper<T>
			{
				Action = param.Action,
				Parameters = param
			}
		};

		logger.LogDebug("[{Mid}] [{Target}:{Action}] sending", mid, target, param.Action);

		// Serialise and send this
		await sock.SendTaskAsync(JsonConvert.SerializeObject(envelope));
	}

	public Task<TResponse> RequestAsync<TResponse, TParam>(string target, TParam param) where TParam : IParam
	{
		// Build message envelope
		Guid mid = Guid.NewGuid();
		Envelope<TParam> envelope = new()
		{
			MessageId = mid,
			Target = target,
			Data = new ActionWrapper<TParam>
			{
				Action = param.Action,
				Parameters = param
			}
		};

#if DEBUG
		// This is in a debug check as reflection here will be costly
		logger.LogDebug("[{Mid}] [{Target}:{Action}] Requesting {ParamType}", mid, target, param.Action, typeof(TParam).FullName);
#endif

		// Send request
		sock.SendTaskAsync(JsonConvert.SerializeObject(envelope));

		// Register in pending responses registry
		return pending.Register<TResponse>(mid);
	}
}