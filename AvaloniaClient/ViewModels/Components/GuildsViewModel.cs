using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AvaloniaClient.Dialog;
using AvaloniaClient.Targets;
using AvaloniaClient.ViewModels.Dialog;
using AvaloniaClient.Views.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Data.Guild;
using EchoLib.Protocol.Exceptions;
using EchoLib.Transport;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.ViewModels.Components;

[SingletonModel]
public partial class GuildsViewModel : ViewModelBase
{
	private readonly ILogger<GuildsViewModel> _logger;
	private readonly DialogService _diag;
	private readonly TargetHub _targets;

	public GuildsViewModel(
		ILogger<GuildsViewModel> logger,
		DialogService diag,
		TargetHub targets
	)
	{
		_logger = logger;
		_diag = diag;
		_targets = targets;

		PropertyChanged += CheckRefreshNeeded;
	}

	private void CheckRefreshNeeded(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(ReloadRequired) && ReloadRequired)
		{
			Reload().Forget(_logger);
		}
	}

	[ObservableProperty] public partial ObservableCollection<JGuild> Guilds { get; set; } = [];
	[ObservableProperty] public partial bool ReloadRequired { get; set; } = true;
	[ObservableProperty] public partial ChatViewModel? Current { get; set; }
	[ObservableProperty] public partial bool GuildSelected { get; set; } = false;

	[RelayCommand]
	public async Task Reload()
	{
		// TODO: Currently there is no persistent ordering information
		IEnumerable<JGuild> net = await _targets.Guilds.Query();

		Guilds = [.. net];
	}


	[RelayCommand]
	public async Task NewGuild()
	{
		string? newName = await _diag.ShowAsync<NewGuildViewModel, string>();

		Snowflake newId = await _targets.Guilds.New(newName!);

		// Set reload requested
		ReloadRequired = true;
		
		// Select the newly created guild
		await SelectGuild(newId);
	}

	[RelayCommand]
	public async Task SelectGuild(Snowflake id)
	{
		_logger.LogInformation("Selected channel {cId}", id);

		Current = new ChatViewModel(
			await _targets.Guilds.Get(id) ?? throw new NotFoundException()
			);
		GuildSelected = true;
	}
}