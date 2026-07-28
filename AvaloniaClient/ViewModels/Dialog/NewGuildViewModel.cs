using System;
using System.Threading.Tasks;
using AvaloniaClient.Dialog;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaClient.ViewModels.Dialog;

[TransientModel]
public partial class NewGuildViewModel(DialogService diag) : ViewModelBase
{
	private readonly DialogService _diag = diag;

	#region Controls

	[ObservableProperty] public partial string GuildName { get; set; }

	#endregion

	[RelayCommand]
	public async Task Submit()
	{
		if (!string.IsNullOrEmpty(GuildName)) diag.Close(GuildName);
	}
}