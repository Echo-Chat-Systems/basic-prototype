using System.ComponentModel;
using AvaloniaClient.ViewModels.Login;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaClient.ViewModels;

[SingletonModel]
public partial class MainViewModel : ViewModelBase
{
	private readonly AppState _state;

	// ReSharper disable once MemberCanBePrivate.Global
	public MainViewModel(AppState state, AuthOverlayViewModel authOverlay)
	{
		_state = state;
		AuthOverlay = authOverlay;

		// Subscribe to relevant state changes
		_state.Remote.PropertyChanged += OnRemoteChange;
	}

	private void OnRemoteChange(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(RemoteState.ServerName):
				// Update local server name
				ConnectedTo = $"Connected to {_state.Remote.ServerName}";
				break;
		}
	}


	#region Observable
	[ObservableProperty] public partial AuthOverlayViewModel AuthOverlay { get; set; }
	[ObservableProperty] public partial string ConnectedTo { get; set; } = "Not connected";
	#endregion


#if DEBUG
	public MainViewModel() : this(new AppState(), new AuthOverlayViewModel())
	{
	}
#endif
}