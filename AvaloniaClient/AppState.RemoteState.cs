using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using EchoLib.Models;
using EchoLib.Models.Data.User;

namespace AvaloniaClient;

public partial class RemoteState : ObservableObject
{
	[ObservableProperty] public partial string ServerName { get; set; } = "";
	[ObservableProperty] public partial JUser? User { get; set; } = null;
}