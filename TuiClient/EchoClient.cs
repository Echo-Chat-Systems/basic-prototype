using EchoLib.Models;
using EchoLib.Models.Misc;
using EchoLib.Models.Params.Auth;
using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TuiClient.Targets;
using WebSocketSharper;

namespace TuiClient;

public sealed class EchoClient
{
	private readonly ILogger<EchoClient> _logger = Program.Services.GetRequiredService<ILogger<EchoClient>>();
	private readonly State _state = Program.Services.GetRequiredService<State>();
	private readonly TargetCollection _targets = Program.Services.GetRequiredService<TargetCollection>();
	private readonly Router _router = Program.Services.GetRequiredService<Router>();

	public EchoClient()
	{
		// Create websocket
		ServerInfoJm server = _state.Local.UserFile!.Server;
		string url = $"ws://{server.Address}:{server.Port}";

		_state.Net.RegisterConnection(new WebSocket(Program.Services.GetRequiredService<ILogger<WebSocket>>(), url, false));
		_state.Net.Socket!.OnMessage += OnMessage;
	}

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