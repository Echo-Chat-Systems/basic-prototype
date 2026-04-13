using System.Windows;
using EchoLib.Core.Routing;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Models.Params.Auth;
using GuiClient.Routing;
using GuiClient.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebSocketSharper;

namespace GuiClient;

public partial class ClientWindow : Window
{
	/// <summary>
	/// Window parent, normally StartupWindow. 
	/// </summary>
	private Window _parent;
	
	private IServiceProvider Services { get; init; }

	private WebSocket Socket { get; init; }
	private StateStore State { get; init; }

	private readonly ILogger<ClientWindow> _logger;
	
	
	public ClientWindow(Window parent, IServiceProvider services)
	{
		_parent = parent;
		Services = services;
		
		// Get required services
		State = Services.GetRequiredService<StateStore>();
		Socket = State.Socket ?? throw new InvalidOperationException($"Socket cannot be null when initialising {nameof(ClientWindow)}");
		
		_logger = Services.GetRequiredService<ILogger<ClientWindow>>();
		
		// Initialise GUI
		InitializeComponent();
		
		// Register the server name change event
		State.ServerNameUpdated += (_, args) =>
		{
			_logger.LogInformation("Server name set to {Name}", args.NewName);

			Application.Current.Dispatcher.BeginInvoke(() => Title = args.NewName);
		}; 
		
		// Register this window as the socket handler
		_logger.LogDebug("Registering client socket listeners");
		Socket.OnOpen += SocketOnOnOpen;
		Socket.OnClose += SocketOnOnClose;
		Socket.OnMessage += SocketOnOnMessage;
		
		// Connect to server
		_logger.LogInformation("Connecting to server...");
		Socket.Connect();
		State.SocketConnected = true;
		_logger.LogInformation("Successfully connected to remote server.");
		
		// Once this window is initialised, hide the parent
		_parent.Hide();
		
		Show();
		
		// Run auth
		Application.Current.Dispatcher.BeginInvoke(Authenticate);
	}

	private async Task Authenticate()
	{
		// Get required services
		Router router = Services.GetRequiredService<Router>();
		AuthTarget tar = router.GetTarget<AuthTarget>() ?? throw new InvalidOperationException();

		// Attempt to login
		try
		{
			await tar.SendSigninStart(
				new RoutingContext(State.Socket!) { Services = Services },
				new SigninStartParameters { Ek = State.UserFile!.Keys.PubEk, Sk = State.UserFile.Keys.PubSk }
			);
		}
		catch (NotFoundException)
		{
			// Prompt user on if they wish to create a new account.
			MessageBoxResult dialog = MessageBox.Show(this, "No Account found on server, create new account?", "Account Not Found", MessageBoxButton.OKCancel);

			if (dialog == MessageBoxResult.Cancel)
			{
				Close();
				return;
			} 
			
			// Spin up a new account window and wait for it to close
			NewAccountWindow newAccWindow = new(Services);
			newAccWindow.ShowDialog();
			
			// Account should be logged in now
		}
		
	}
	

	private void SocketOnOnMessage(object? sender, MessageEventArgs e)
	{
		// Build a new context
		RoutingContext ctx = new(Socket) { Services = Services };

		// Unpack message event 
		_logger.LogDebug("Message received, attempting to unpack");
		MessageEnvelope<object>? envelope = null;
		try
		{
			envelope = JsonConvert.DeserializeObject<MessageEnvelope<object>>(e.Data);
		}
		catch (JsonReaderException)
		{
			goto Fail;
		}

		if (envelope is null) goto Fail;

		_logger.LogDebug("Unpacked message {Target}", envelope.Target);

		// Route message
		_ = Services.GetRequiredService<Router>().RouteAsync(ctx, envelope);
		return;

		Fail:
		_logger.LogError("Invalid envelope received!");
	}

	private void SocketOnOnClose(object? sender, CloseEventArgs e)
	{
		State.SocketConnected = false;
		DialogHelpers.ShowError(_parent, "Socket unexpectedly closed!");
		Close();
	}

	private void SocketOnOnOpen(object? sender, EventArgs e)
	{
		// Create new ctx
		RoutingContext ctx = new(Socket) { Services = Services };

		// Get router
		Router router = Services.GetRequiredService<Router>();
		
		// Send hello message
		_logger.LogDebug("Sending client-hello");
		router.GetTarget<AuthTarget>()?.SendHello(ctx, new ClientHelloParameters { Id = State.UserFile!.Keys.PubSk });
	}


	protected override void OnClosed(EventArgs e)
	{
		// Re-show the parent window
		_parent.Show();
	}
}