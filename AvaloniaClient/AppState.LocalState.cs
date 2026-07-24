using CommunityToolkit.Mvvm.ComponentModel;
using EchoLib.Models;
using EchoLib.Models.Data.User;

namespace AvaloniaClient;

public partial class LocalState : ObservableObject
{
	[ObservableProperty] public partial JUserFile? UserFile { get; set; } = null!;

	[NotifyPropertyChangedFor(nameof(FrontendReady))]
	[ObservableProperty]
	public partial AuthStates? AuthState { get; set; }

	public bool FrontendReady => AuthState == AuthStates.FrontendReady;

	[ObservableProperty] public partial JProfile? Profile { get; set; } = null;

	public enum AuthStates
	{
		UnlockRequired,
		StartConnect,
		Connecting,
		Registering,
		StartAuth,
		Authenticating,
		CreatingNewAccount,
		FrontendReady,
		Failed
	}
}