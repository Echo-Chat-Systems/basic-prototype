using System;
using System.Threading.Tasks;
using AvaloniaClient.Dialog;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaClient.ViewModels.Dialog;

[TransientModel]
public partial class NewChannelViewModel(DialogService diag) : ViewModelBase
{
	private readonly DialogService _diag = diag;

	#region Controls

	[ObservableProperty] public partial string ChannelName { get; set; }

	#endregion

	[RelayCommand]
	public async Task Submit()
	{
		if (!string.IsNullOrEmpty(ChannelName)) diag.Close(ChannelName);
	}
}