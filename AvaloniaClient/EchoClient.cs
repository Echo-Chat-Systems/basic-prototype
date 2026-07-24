using System;
using System.IO;
using System.Threading.Tasks;
using AvaloniaClient.Targets;
using EchoLib.Models.Misc;
using EchoLib.Protocol;
using EchoLib.Routing;
using EchoLib.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharper;

namespace AvaloniaClient;

public sealed class EchoClient
{
	private IServiceProvider _services;
	private readonly ILogger<EchoClient> _logger;
	private readonly AppState _state;
	private readonly TargetHub _targets;
	private readonly Router _router;

	public EchoClient
	(
		IServiceProvider services,
		ILogger<EchoClient> logger,
		AppState state,
		TargetHub targets,
		Router router
	)
	{
		_services = services;
		_logger = logger;
		_state = state;
		_targets = targets;
		_router = router;


		// Create websocket
		ServerInfoJm server = _state.Local.UserFile!.Server;
		string url = $"ws://{server.Address}:{server.Port}";

		_state.Net.Socket = new WebSocket(services.GetRequiredService<ILogger<WebSocket>>(), url, false);
		_state.Net.Socket.OnMessage += OnMessage;
	}

	public IMessageEndpoint BuildEndpoint => new WebsocketEndpoint(_state.Net.Socket, _services);


	private void OnMessage(object? sender, MessageEventArgs e)
	{
		// Unpack message event
		_logger.LogDebug("Message received, attempting to unpack");
		Envelope<JToken>? envelope;
		try
		{
			envelope = JsonConvert.DeserializeObject<Envelope<JToken>>(e.Data);
		}
		catch (JsonReaderException)
		{
			goto Fail;
		}

		if (envelope is null) goto Fail;

		_logger.LogDebug("Unpacked message {Target}", envelope.Target);

		// Route message
		_router.Receive(envelope, _state.Net.Socket!);
		return;

		Fail:
		_logger.LogError("Invalid envelope received!");
	}

	public async Task Connect()
	{
		if (_state.Net.Socket is null) throw new InvalidDataException($"Unset {nameof(_state.Net.Socket)}");

		await _state.Net.Socket.ConnectTaskAsync();
	}
}