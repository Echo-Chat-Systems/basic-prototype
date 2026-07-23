using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace TuiClient.Windows;

public sealed class ConnectingWindow : View
{
	private readonly State _state = Program.Services.GetRequiredService<State>();

	private static readonly Label ConnectingStatusLabel = new Label
	{
		Text = "",

	};

	public ConnectingWindow()
	{
		// Check if user file is null
		if (_state.Local.UserFile == null) throw new InvalidOperationException($"Unable to create {nameof(ConnectingWindow)} without valid UserFile");

		Title = "Connecting...";

		Add(new Label
		{
			Text = "Connecting..",
			X = Pos.Center(),
			Y = Pos.Top(this),
			Width = Dim.Fill(),
			Height = Dim.Percent(10)
		});

		Add(ConnectingStatusLabel);

		_state.Remote.OnServerNameChanged += args => ConnectingStatusLabel.Text = $"Connected to {args.NewName}!";

		// Build the echo client
		_state.ProtocolClient = new EchoClient();
	}
}