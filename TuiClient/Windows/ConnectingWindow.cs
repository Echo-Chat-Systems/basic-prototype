using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.ViewBase;

namespace TuiClient.Windows;

public sealed class ConnectingWindow : View
{
	private readonly State _state = Program.Services.GetRequiredService<State>();

	public ConnectingWindow()
	{
		// Check if user file is null
		if (_state.Local.UserFile == null) throw new InvalidOperationException($"Unable to create {nameof(ConnectingWindow)} without valid UserFile");

		Title = "Connecting...";
	}
}