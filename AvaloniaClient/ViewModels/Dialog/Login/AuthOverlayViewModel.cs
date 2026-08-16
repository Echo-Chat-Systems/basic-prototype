using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AvaloniaClient.Dialog;
using AvaloniaClient.Managers;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.ViewModels.Dialog.Login;

[TransientModel]
public partial class AuthOverlayViewModel : ViewModelBase
{
	private readonly IServiceProvider _services;
	private readonly ILogger<AuthOverlayViewModel> _logger;

	private readonly AppState _state;
	private readonly IDialogService _dialogs;

	[ObservableProperty] public partial object? CurrentPage { get; set; }
	[ObservableProperty] public partial bool Visible { get; set; } = true;
	[ObservableProperty] public partial ObservableCollection<string> AuthLogs { get; set; }
	[ObservableProperty] public partial string AuthStage { get; set; } = "Not Connected";


	// ReSharper disable once MemberCanBePrivate.Global
	public AuthOverlayViewModel
	(
		ILogger<AuthOverlayViewModel> logger,
		AppState state,
		AuthManager authManager,
		IServiceProvider services,
		DialogService dialogs
	)
	{
		_services = services;
		_dialogs = dialogs;
		_logger = logger;
		_state = state;

		AuthLogs = authManager.Logs;

		_state.Local.PropertyChanged += LocalStateChanged;

		// Set state to logging in
		_state.Local.AuthState = LocalState.AuthStates.UnlockRequired;
		CurrentPage = _services.GetRequiredService<UnlockFileViewModel>();
	}

	private void LocalStateChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(LocalState.AuthState)) UpdateView();
	}

	private void UpdateView()
	{
		switch (_state.Local.AuthState)
		{
			case LocalState.AuthStates.UnlockRequired:
				CurrentPage = _services.GetRequiredService<UnlockFileViewModel>();
				break;
			case LocalState.AuthStates.StartConnect:
				AuthStage = "Starting Connection";
				break;
			case LocalState.AuthStates.Connecting:
				AuthStage = "Connecting";
				break;
			case LocalState.AuthStates.CreatingNewAccount:
				CurrentPage = _services.GetRequiredService<NewAccountViewModel>();
				AuthStage = "Waiting for new account creation";
				break;
			case LocalState.AuthStates.Authenticating:
				AuthStage = "Authenticating with server";
				break;
			case LocalState.AuthStates.Preloading:
				// Skip preloading for now and go straight to ready
				_state.Local.AuthState = LocalState.AuthStates.FrontendReady;
				break;
			case LocalState.AuthStates.FrontendReady:
				AuthStage = "Finishing up";
				_dialogs.Close(true);
				break;
		}
	}

	private object? Complete(bool result)
	{
		_dialogs.Close(result);
		return null;
	}


#if DEBUG
	public AuthOverlayViewModel() : this(null!, new AppState(), null!, null!, null!)
	{
		AuthLogs = ["Example line1", "Example line 2"];
	}

#endif
}