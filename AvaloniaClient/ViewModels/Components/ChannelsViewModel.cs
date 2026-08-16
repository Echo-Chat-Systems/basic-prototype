using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using AvaloniaClient.Dialog;
using AvaloniaClient.Targets;
using AvaloniaClient.ViewModels.Dialog;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Data.Guild;
using EchoLib.Transport;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.ViewModels.Components;

[TransientModel]
public partial class ChannelsViewModel : ViewModelBase
{
	
	private readonly ILogger<ChannelsViewModel> _logger;
	private readonly DialogService _diag;
	private readonly TargetHub _targets;
	
	public ChannelsViewModel(
		ILogger<ChannelsViewModel> logger,
		DialogService diag,
		TargetHub targets
	)
	{
		_logger = logger;
		_diag = diag;
		_targets = targets;

		PropertyChanged += CheckRefreshNeeded;
	}

	public JGuild Guild { get; set; } = null!;
	public ObservableCollection<JChannel> Channels { get; set; } = null!;

	[ObservableProperty] public partial bool ReloadRequired { get; set; } = true;
	
	[RelayCommand]
	public async Task SelectChannel(Snowflake channelId)
	{
	}
	
	private void CheckRefreshNeeded(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(ReloadRequired) && ReloadRequired) Reload().Forget(_logger);
	}


	[RelayCommand]
	public async Task NewChannel()
	{
		string? newName = await _diag.ShowAsync<NewChannelViewModel, string>();
		Snowflake newId = await _targets.Channels.New(Guild.Id, newName!);

		// Set reload requested
		ReloadRequired = true;

		// Select the newly created guild
		await SelectChannel(newId);
	}

	[RelayCommand]
	public async Task Reload()
	{
	}
}