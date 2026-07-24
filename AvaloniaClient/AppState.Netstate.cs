using CommunityToolkit.Mvvm.ComponentModel;
using WebSocketSharper;

namespace AvaloniaClient;

public partial class NetState : ObservableObject
{
	[ObservableProperty] public partial WebSocket Socket { get; set; } = null!;
	[ObservableProperty] public partial EchoClient Client { get; set; } = null!;
}