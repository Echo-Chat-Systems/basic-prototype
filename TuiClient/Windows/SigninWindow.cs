using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Terminal.Gui.ViewBase;
using TuiClient.Targets;

namespace TuiClient.Windows;

public class SigninWindow : View
{
	private readonly ILogger<SigninWindow> _logger = Program.Services.GetRequiredService<ILogger<SigninWindow>>();
	private readonly State _state = Program.Services.GetRequiredService<State>();
	private readonly TargetCollection _targets = Program.Services.GetRequiredService<TargetCollection>();
	private readonly WindowManager _windowManager = Program.Services.GetRequiredService<WindowManager>();

	public SigninWindow()
	{
		Title = $"Sign In to {_state.Remote.ServerName}";
	}
}