using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Channel;

namespace AvaloniaClient.ViewModels.Components;

[TransientModel]
public partial class ChannelsViewModel : ViewModelBase
{
	public required ObservableCollection<JChannel> Channels { get; set; }

	[RelayCommand]
	public async Task SelectChannel(Snowflake channelId)
	{

	}


	[RelayCommand]
	public async Task NewChannel()
	{

	}

	[RelayCommand]
	public async Task Reload()
	{

	}
}