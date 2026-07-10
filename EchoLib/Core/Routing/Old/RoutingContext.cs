using EchoLib.Protocol.Models.Params;
using EchoLib.Protocol.Models.Params.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebSocketSharper;
using WebSocketSharper.Net.WebSockets;


namespace EchoLib.Core.Routing;

public sealed class RoutingContext
{
	/// <summary>
	/// Service provider.
	/// </summary>
	public required IServiceProvider Services { get; init; }

	/// <summary>
	/// Websocket connection.
	/// </summary>
	public WebSocket Socket { get; init; }

	/// <summary>
	/// Websocket context.
	/// </summary>
	public WebSocketContext? FullContext { get; set; }

	public Guid? RequestId { get; } = null;

	public CancellationToken Ct { get; }

	public RoutingContext(WebSocketContext fullContext)
	{
		FullContext = fullContext;
		Socket = fullContext.WebSocket;
	}

	public RoutingContext(WebSocket socket)
	{
		Socket = socket;
	}

	private ILogger<RoutingContext> Logger => Services.GetRequiredService<ILogger<RoutingContext>>();

	public async Task ReplyAsync<TParams>(ITarget invoker, TParams parameters) where TParams : IParam
	{
		Logger.LogDebug("Attempting to send {Target} {Action}", invoker.Name, parameters.Action);

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
		Logger.LogDebug("Attempting to send error {Action}", envelope.Data.Action);

		Socket.SendAsync(JsonConvert.SerializeObject(envelope), null);
	}
}