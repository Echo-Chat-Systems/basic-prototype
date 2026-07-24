using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AvaloniaClient.Targets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EchoLib.Models.Data.User;
using EchoLib.Models.Params.Auth;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.ViewModels.Login;

[TransientModel]
public partial class NewAccountViewModel : ViewModelBase
{
	private readonly ILogger<NewAccountViewModel> _logger;
	private readonly AppState _state;
	private readonly TargetHub _targets;

	public NewAccountViewModel
	(
		ILogger<NewAccountViewModel> logger,
		AppState state,
		TargetHub targets
	)
	{
		_logger = logger;
		_state = state;
		_targets = targets;
	}

	#region Props

	// Required
	[ObservableProperty] public required partial string Username { get; set; }
	[ObservableProperty] public required partial ushort Tag { get; set; }
	[ObservableProperty] public partial string? Pronouns { get; set; }

	// Optional
	[ObservableProperty] public partial string? Bio { get; set; }
	[ObservableProperty] public partial string? CustomCss { get; set; }
	[ObservableProperty] public partial string? PfpLink { get; set; }
	[ObservableProperty] public partial string? BannerLink { get; set; }
	[ObservableProperty] public partial bool SaveCurrentTimezone { get; set; } = false;

	#endregion

	private bool Validate()
	{
		return true;
	}

	[RelayCommand]
	public async Task CreateAsync()
	{
		if (!Validate()) return;

		// Create a new profile model
		JProfile profile = new()
		{
			Username = Username,
			Tag = Tag,
			Pronouns = Pronouns,
			Bio = Bio,
			Css = CustomCss,
			Pfp = PfpLink,
			Banner = BannerLink,
			Timezone = "nanya", // TODO: Actually implement tz saving
		};

		SignupCompleteParameters res =await _targets.Auth.Signup(profile);
		_state.Local.Profile = profile;

		// Trigger auth
		_state.Local.AuthState = LocalState.AuthStates.StartAuth;
	}
}