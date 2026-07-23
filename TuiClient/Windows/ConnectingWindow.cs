using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace TuiClient.Windows;

public sealed class ConnectingWindow : View
{
	private readonly State _state = Program.Services.GetRequiredService<State>();

	public ConnectingWindow()
	{
		// Check if user file is null
		if (_state.Local.UserFile == null) throw new InvalidOperationException($"Unable to create {nameof(ConnectingWindow)} without valid UserFile");

		Title = "Connecting...";

		Add(new Label
		{
			Text = "Connecting..",
			X = Pos.Center(),
			Y = Pos.Center(),
			Width = Dim.Percent(50),
			Height = Dim.Percent(50)
		});
	}
}