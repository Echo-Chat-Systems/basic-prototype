using EchoLib.Models.Data.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Terminal.Gui.App;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using TuiClient.Components;

namespace TuiClient.Windows;

public class CreateAccountWindow : View
{
	private readonly ILogger<CreateAccountWindow> _logger = Program.Services.GetRequiredService<ILogger<CreateAccountWindow>>();
	private CreateAccountWindowModel _viewModel = new();

	public CreateAccountWindow()
	{
		Title = "Create account";
		CanFocus = true;

		Add(_viewModel.Stack);

		_viewModel.Stack.TabStop = TabBehavior.TabGroup;
		_viewModel.Stack.Width = Dim.Fill();
		_viewModel.Stack.Height = Dim.Fill();

		_viewModel.BtnCreate.Accepted += Submit;
	}

	private void Submit(object? sender, CommandEventArgs e)
	{
		_logger.LogDebug("{} called", nameof(Submit));

		// Create a new user model and create account
	}
}

public class CreateAccountWindowModel
{
	private readonly ILogger<CreateAccountWindowModel> _logger = Program.Services.GetRequiredService<ILogger<CreateAccountWindowModel>>();

	/*
	 public class JProfileModel
	   {
	    [JsonProperty("username")] public required string Username { get; init; }
	    [JsonProperty("tag")] public required ushort Tag { get; set; }
	    [JsonProperty("pronouns")] public string? Pronouns { get; set; }
	    [JsonProperty("bio")] public string? Bio { get; set; }
	    [JsonProperty("css")] public string? Css { get; set; }
	    [JsonProperty("pfp")] public string? Pfp { get; set; }
	    [JsonProperty("bammer")] public string? Banner { get; set; }
	    [JsonProperty("timezone")] public string? Timezone { get; set; }
	    [JsonProperty("status")] public JStatusModel? Status { get; init; }
	   }

	   public class JStatusModel
	   {
	    [JsonProperty("type")] public required string Type { get; init; }
	    [JsonProperty("text")] public required string Text { get; init; }
	   }
	 */

	#region Controls

	// Stackview
	public readonly StackView Stack = new(StackView.Direction.TopToBottom);

	// Username
	public readonly Label LUsername = new() { Text = "Username *" };
	public readonly TextField FUsername = new() { TabStop = TabBehavior.TabStop };
	public string Username => FUsername.Text;

	// Tag
	public readonly Label LTag = new() { Text = "Tag *" };
	public readonly TextField FTag = new() { TabStop = TabBehavior.TabStop };
	public ushort? Tag => ushort.TryParse(FTag.Text, out ushort result) ? result : null;


	// Submit Button
	public readonly Button BtnCreate = new() { Text = "Submit", TabStop = TabBehavior.TabStop };

	// Error display
	private readonly Label LError = new();


	private View[] Controls =>
	[
		LUsername, FUsername,
		LTag, FTag,

		BtnCreate,
		LError
	];

	#endregion

	public CreateAccountWindowModel(View[]? before = null, View[]? after = null)
	{
		Stack.AddControl(
			[
				.. before ?? [],
				.. Controls,
				.. after ?? []
			]
		);

		FTag.ValueChanging += FTagVerify;
		BtnCreate.Accepting += BtnCreateVerify;
	}

	private void BtnCreateVerify(object? sender, CommandEventArgs e)
	{
		// Verification logic here
		_logger.LogDebug("{} called", nameof(BtnCreateVerify));

		// Ensure tag works
		if (Tag is null)
		{
			LError.Text = "Tag invalid";
			e.Handled = true;
			return;
		}

		LError.Text = "";
		e.Handled = false;
	}

	private void FTagVerify(object? sender, ValueChangingEventArgs<string?> e)
	{
		// Allow the value to be set to null
		if (e.NewValue == "") return;

		// Ensure that the tag fits a ushort
		bool valid = ushort.TryParse(e.NewValue, out _);

		_logger.LogDebug("{Before} -> {After} : {Valid}", e.CurrentValue, e.NewValue, valid);

		e.Handled = !valid;
	}
}