using CommunityToolkit.Mvvm.ComponentModel;
using WebSocketSharper;

namespace AvaloniaClient.State;

public partial class SocketState : ObservableObject
{
	[ObservableProperty] public partial WebSocket Socket { get; set; } = null!;

	public bool IsConnected => Socket is { IsAlive: true };
}