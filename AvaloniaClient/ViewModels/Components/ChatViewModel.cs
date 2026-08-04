using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Data.Guild;
using Microsoft.Extensions.DependencyInjection;

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

		ChannelsData = Ioc.Default.GetRequiredService<ChannelsViewModel>();
		ChannelsData.Channels = Channels;
		ChannelsData.Guild = guild;
	}
}

public partial class ChatMessageModel : ObservableObject
{
	[ObservableProperty] public partial string Text { get; set; } = "Invalid message";
}