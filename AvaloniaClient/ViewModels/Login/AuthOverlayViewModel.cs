using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AvaloniaClient.Managers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.ViewModels.Login;

[TransientModel]
public partial class AuthOverlayViewModel : ViewModelBase
{
	private readonly IServiceProvider _services;
	private readonly ILogger<AuthOverlayViewModel> _logger;

	private readonly AppState _state;
	[ObservableProperty] public partial object? CurrentPage { get; set; }
	[ObservableProperty] public partial bool Visible { get; set; } = true;
	[ObservableProperty] public partial ObservableCollection<string> AuthLogs { get; set; }



	// ReSharper disable once MemberCanBePrivate.Global
	public AuthOverlayViewModel
	(
		ILogger<AuthOverlayViewModel> logger,
		AppState state,
		AuthManager authManager,
		IServiceProvider services
	)
	{
		_services = services;
		_logger = logger;
		_state = state;

		AuthLogs = authManager.Logs;

		_state.Local.PropertyChanged += LocalStateChanged;

		// Set state to logging in
		_state.Local.AuthState = LocalState.AuthStates.UnlockRequired;
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
				break;
			case LocalState.AuthStates.Connecting:
				break;
			case LocalState.AuthStates.CreatingNewAccount:
				break;
			case LocalState.AuthStates.Authenticating:
				break;
			case LocalState.AuthStates.FrontendReady:
				break;
			case null:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}


#if DEBUG
	public AuthOverlayViewModel() : this(null!, new AppState(), null!, null!)
	{
		AuthLogs = ["Example line1", "Example line 2"];
	}

#endif
}