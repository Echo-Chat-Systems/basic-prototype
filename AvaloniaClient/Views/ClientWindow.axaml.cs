using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaClient.State;
using CommunityToolkit.Mvvm.DependencyInjection;
using EchoLib.Protocol;
using EchoLib.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharper;

namespace AvaloniaClient.Views;

public partial class ClientWindow : LoggableWindow
{
	private AppState _state;
	private Router _router;

	public ClientWindow()
	{
		_state = Ioc.Default.GetRequiredService<AppState>();
		_router = Ioc.Default.GetRequiredService<Router>();

		InitializeComponent();

		// Initialise socket
		_state.SocketState.Socket = new WebSocket(
			Ioc.Default.GetRequiredService<ILogger<WebSocket>>(),
			$"ws://{_state.UserFile!.Server.Address}:{_state.UserFile.Server.Port}",
			false
			);

		_state.SocketState.Socket.OnMessage += OnMessage;
	}

	public async Task Run()
	{
		// Connect to socket
		_state.SocketState.Socket.ConnectAsync();

		while (!_state.SocketState.IsConnected) {}  // Wait for socket to be connected

	}

	private void OnMessage(object? sender, MessageEventArgs e)
	{
		// Unpack message event
		Logger.LogDebug("Message received, attempting to unpack");
		Envelope<JToken>? envelope = null;
		try
		{
			envelope = JsonConvert.DeserializeObject<Envelope<JToken>>(e.Data);
		}
		catch (JsonReaderException)
		{
			goto Fail;
		}

		if (envelope is null) goto Fail;

		Logger.LogDebug("Unpacked message {Target}", envelope.Target);

		// Route message
		Ioc.Default.GetRequiredService<Router>().Receive(envelope, _state.SocketState.Socket);
		return;

		Fail:
		Logger.LogError("Invalid envelope received!");
	}


}