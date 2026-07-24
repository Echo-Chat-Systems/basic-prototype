using AvaloniaClient.ViewModels.Login;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaClient.ViewModels;

[SingletonModel]
public partial class MainViewModel(AppState state, AuthOverlayViewModel authOverlay) : ViewModelBase
{
	[ObservableProperty] public partial AppState State { get; set; } = state;
	[ObservableProperty] public partial AuthOverlayViewModel AuthOverlay { get; set; } = authOverlay;

#if DEBUG
	public MainViewModel() : this(new AppState(), new AuthOverlayViewModel()){}
#endif

}

