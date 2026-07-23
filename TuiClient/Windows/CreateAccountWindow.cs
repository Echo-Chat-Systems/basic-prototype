using EchoLib.Models.Data.User;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using TuiClient.Components;

namespace TuiClient.Windows;

public class CreateAccountWindow : Runnable<JProfile>
{
	private CreateAccountWindowModel _viewModel = new();

	public CreateAccountWindow()
	{
		Add(_viewModel.All());
	}
}

public class CreateAccountWindowModel
{

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

	public readonly Label LUsername = new() { Text = "Username *" };
	public readonly TextField FUsername = new();

	public readonly Label LTag = new() { Text = "Tag *" };
	public readonly TextField FTag = new();

	public readonly Button BtnCreate = new() { Text = "Submit" };

	private View[] Controls => [
		LUsername, FUsername,
		LTag, FTag,

		BtnCreate
	];

	#endregion

	public CreateAccountWindowModel()
	{
		FTag.ValueChanging += FTagVerify;
	}

	private void FTagVerify(object? sender, ValueChangingEventArgs<string?> valueChangingEventArgs)
	{
		// Ensure that the tag fits a ushort
		valueChangingEventArgs.Handled = ushort.TryParse(FTag.Value, out _);
	}

	public View All()
	{
		StackView stack = new(StackView.Direction.TopToBottom)
		{
			Width = Dim.Fill(),
			Height = Dim.Fill(),
		};
		stack.AddControl(Controls);

		return stack;
	}

}