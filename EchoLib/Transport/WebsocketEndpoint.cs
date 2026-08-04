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

	private bool Preflight(out Exception? ex)
	{
		if (!sock.IsAlive)
		{
			ex = new InvalidOperationException();
			return false;
		}

		ex = null;
		return true;
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

		logger.LogError("[{Mid}] [error:error] [-- Prepared] [{Error}]", mid, err.Message);
		if (!Preflight(out Exception? exception)) throw exception!;
		await sock.SendTaskAsync(JsonConvert.SerializeObject(exEnvelope));
		logger.LogError("[{Mid}] [error:error] [-> Sent] [{Error}]", mid, err.Message);
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

		logger.LogDebug("[{Mid}] [{Target}:{Action}] [-- Prepared]", mid, target, param.Action);
		if (!Preflight(out Exception? exception)) throw exception!;
		await sock.SendTaskAsync(JsonConvert.SerializeObject(envelope));
		logger.LogDebug("[{Mid}] [{Target}:{Action}] [-> Sent]", mid, target, param.Action);
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
		logger.LogDebug("[{Mid}] [{Target}:{Action}] [-- Prepared] Requesting <-> {ParamType}", mid, target, param.Action, typeof(TParam).FullName);
#endif
		if (!Preflight(out Exception? exception)) return Task.FromException<TResponse>(exception!);
		// Send request
		sock.SendTaskAsync(JsonConvert.SerializeObject(envelope));

#if DEBUG
		logger.LogDebug("[{Mid}] [{Target}:{Action}] [-> Sent] Expecting <-> {ParamType}", mid, target, param.Action, typeof(TParam).FullName);
#endif

		// Register in pending responses registry
		Task<TResponse> t = pending.Register<TResponse>(mid);
#if DEBUG
		logger.LogDebug("[{Mid}] [{Target}:{Action}] [-- Waiting] Expect <-> {ParamType}", mid, target, param.Action, typeof(TParam).FullName);
#endif
		return t;
	}
}