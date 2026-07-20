using CommunityToolkit.Mvvm.ComponentModel;
using EchoLib.Models;

namespace AvaloniaClient.State;


public partial class AppState : ObservableObject
{
	[ObservableProperty] public partial UserFileJm? UserFile { get; set; } = null!;

	public bool IsUserFileLoaded => UserFile != null;

	public SocketState SocketState { get; } = new SocketState();
}

