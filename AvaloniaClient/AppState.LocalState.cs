using CommunityToolkit.Mvvm.ComponentModel;
using EchoLib.Models;

namespace AvaloniaClient;

public partial class LocalState : ObservableObject
{
	[ObservableProperty] public partial JUserFile? UserFile { get; set; } = null!;

	[NotifyPropertyChangedFor(nameof(FrontendReady))]
	[ObservableProperty]
	public partial AuthStates? AuthState { get; set; }

	public bool FrontendReady => AuthState == AuthStates.FrontendReady;

	public enum AuthStates
	{
		UnlockRequired,
		StartConnect,
		Connecting,
		Registering,
		Authenticating,
		CreatingNewAccount,
		FrontendReady,
		Failed
	}
}