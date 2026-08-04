using CommunityToolkit.Mvvm.ComponentModel;
using EchoLib.Models;

namespace AvaloniaClient;

public partial class AppState : ObservableObject
{
	public LocalState Local { get; } = new();
	public NetState Net { get; } = new();
	public RemoteState Remote { get; } = new();
}