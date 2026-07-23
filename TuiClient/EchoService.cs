using EchoLib.Models;
using EchoLib.Models.Misc;
using EchoLib.Models.Params.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TuiClient.Targets;
using WebSocketSharper;

namespace TuiClient;

public sealed class EchoClient
{
	private readonly State _state = Program.Services.GetRequiredService<State>();
	private readonly TargetCollection _targets = Program.Services.GetRequiredService<TargetCollection>();

	public Task StartupTask { get; }

	public EchoClient()
	{
		// Create websocket
		ServerInfoJm server = _state.Local.UserFile!.Server;
		string url = $"ws://{server.Address}:{server.Port}";

		_state.Net.RegisterConnection(new WebSocket(Program.Services.GetRequiredService<ILogger<WebSocket>>(), url, false));

		// Perform startup jobs
		StartupTask = Connect();
	}

	private async Task Connect()
	{
		if (_state.Net.Socket is null) throw new InvalidDataException($"Unset {nameof(_state.Net.Socket)}");

		await _state.Net.Socket.ConnectTaskAsync();

		// Now that socket is connected, run auth login
		ServerHelloParameters serverHello = await _targets.Auth.SendHello(_state.Net.Endpoint!, new ClientHelloParameters
		{
			KeyPair = _state.Keys.ToPublicKeyPair()
		});

		_state.Remote.ServerName = serverHello.ServerName;

		// Login

	}
}