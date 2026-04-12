using EchoLib.Models.Params;
using EchoLib.Models.Params.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebSocketSharper;
using WebSocketSharper.Net.WebSockets;


namespace EchoLib.Core.Routing;

public class RoutingContext
{
	public required IServiceProvider Services { get; init; }

	public WebSocket Socket { get; init; }

	public WebSocketContext? FullContext { get; set; }

	public RoutingContext(WebSocketContext fullContext)
	{
		FullContext = fullContext;
		Socket = fullContext.WebSocket;
	}

	public RoutingContext(WebSocket socket)
	{
		Socket = socket;
	}

	private ILogger<RoutingContext> _logger => Services.GetRequiredService<ILogger<RoutingContext>>();

	public async Task SendAsync<TParams>(ITarget invoker, TParams parameters) where TParams : IParam
	{
		_logger.LogDebug("Attempting to send {Target} {Action}", invoker.Name, parameters.Action);

		// Serialise message
		MessageEnvelope<TParams> envelope = new()
		{
			Data = new ActionWrapper<TParams> { Action = parameters.Action, Params = parameters },
			Target = invoker.Name
		};

		Socket.SendAsync(JsonConvert.SerializeObject(envelope), null);
	}

	public async Task SendError(MessageEnvelope<ErrorParameters> envelope)
	{
		_logger.LogDebug("Attempting to send error {Action}", envelope.Data.Action);

		Socket.SendAsync(JsonConvert.SerializeObject(envelope), null);
	}
}