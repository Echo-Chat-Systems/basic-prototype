using System;
using System.ComponentModel;
using AvaloniaClient.Dialog;
using AvaloniaClient.ViewModels.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaClient.ViewModels;

[SingletonModel]
public partial class MainViewModel : ViewModelBase
{
	private readonly AppState _state;
	public DialogService Dialogs { get; set; }


	// ReSharper disable once MemberCanBePrivate.Global
	public MainViewModel(AppState state, DialogService dialogService)
	{
		_state = state;
		Dialogs = dialogService;

		GuildsVm = Ioc.Default.GetRequiredService<GuildsViewModel>();

		// Subscribe to relevant state changes
		_state.Remote.PropertyChanged += OnRemoteChange;
		Dialogs.PropertyChanged += (sender, args) =>
		{
			Console.WriteLine(
				$"Dialog property changed: {args.PropertyName}");

			Console.WriteLine(
				$"CurrentDialog: {Dialogs.CurrentDialog?.GetType().FullName}");

			Console.WriteLine(
				$"IsOpen: {Dialogs.IsOpen}");
		};
	}

	private void OnRemoteChange(object? sender, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(RemoteState.ServerName):
				// Update local server name
				ConnectedTo = $"Connected to {_state.Remote.ServerName}";
				break;
			case nameof(RemoteState.User):
				Username = $"{_state.Remote.User!.Profile.Username}#{_state.Remote.User.Profile.Tag}";
				break;
		}
	}


	#region Observable

	// [ObservableProperty] public partial AuthOverlayViewModel AuthOverlay { get; set; }
	[ObservableProperty] public partial string ConnectedTo { get; set; } = "Not connected";
	[ObservableProperty] public partial string? Username { get; set; }

	public GuildsViewModel GuildsVm { get; }

	#endregion


#if DEBUG
	public MainViewModel() : this(new AppState(), new DialogService(new ServiceCollection().BuildServiceProvider()))
	{
	}
#endif
}