using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Data.Guild;

namespace AvaloniaClient.ViewModels.Components;

[TransientModel]
public partial class ChatViewModel : ViewModelBase
{
	[ObservableProperty] public partial JChannel? Channel { get; set; }

	public ObservableCollection<JChannel> Channels { get; }
	public ChannelsViewModel ChannelsData { get; }

	public ChatViewModel(JGuild guild)
	{
		Channels = [.. guild.Channels];

		ChannelsData = new ChannelsViewModel
		{
			Channels = Channels
		};
	}
}

public partial class ChatMessageModel : ObservableObject
{
	[ObservableProperty] public partial string Text { get; set; } = "Invalid message";
}