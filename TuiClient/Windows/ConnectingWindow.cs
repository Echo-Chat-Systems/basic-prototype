using EchoLib.Models.Params.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Terminal.Gui.App;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using TuiClient.Targets;

namespace TuiClient.Windows;

public sealed class ConnectingWindow : View
{
	private readonly ILogger<ConnectingWindow> _logger = Program.Services.GetRequiredService<ILogger<ConnectingWindow>>();
	private readonly State _state = Program.Services.GetRequiredService<State>();
	private readonly TargetCollection _targets = Program.Services.GetRequiredService<TargetCollection>();
	private readonly WindowManager _windowManager = Program.Services.GetRequiredService<WindowManager>();

	private static readonly TextField LogField = new()
	{
		Text = "Connecting",
		X = Pos.Center(),
		Y = 0,
		Width = Dim.Fill(),
		Height = 1,
	};

	private static readonly Button ConnectButton = new()
	{
		Text = "Connect!",
		X = Pos.Center(),
		Y = Pos.Bottom(LogField) + 1,
		Width = Dim.Fill(),
		Height = 2,
		TabStop = TabBehavior.TabStop
	};

	public ConnectingWindow()
	{

		// Check if user file is null
		if (_state.Local.UserFile == null) throw new InvalidOperationException($"Unable to create {nameof(ConnectingWindow)} without valid UserFile");

		Title = "Connecting...";

		Add(LogField, ConnectButton);

		ConnectButton.Accepted += ConnectPressed;
		_state.Remote.OnServerNameChanged += args => LogField.Text += $"Connected to {args.NewName}!\nAttempting to log in using {_state.Keys.PubSk}...\n";
		_targets.Auth.OnSigninStageChanged += args => LogField.Text += $"Signin state updated: {args.Now}";
	}

	private async void ConnectPressed(object? sender, CommandEventArgs commandEventArgs)
	{
		// Build the echo client
		try
		{
			_state.ProtocolClient = new EchoClient();
			await _state.ProtocolClient.Connect();
		}
		catch (Exception ex)
		{
			_logger.LogCritical("Failed to connect to server: {ErrorName}\n{Stacktrace}", ex.Message, ex.StackTrace);
			// ReSharper disable once AsyncVoidThrowException  // We want to rethrow here, the application has failed TODO: Something other than this
			throw;
		}

		// Now that socket is connected, run auth login
		ServerHelloParameters serverHello = await _targets.Auth.SendHello(_state.Net.Endpoint!, new ClientHelloParameters
		{
			KeyPair = _state.Keys.ToPublicKeyPair()
		});

		_state.Remote.ServerName = serverHello.ServerName;

		// Connected successfully, show SigninWindow
		_windowManager.Show<SigninWindow>(GuiBootstrapper.Root);
	}
}