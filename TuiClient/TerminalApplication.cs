using EchoLib.Core;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.App;
using Terminal.Gui.Drivers;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using TuiClient.Targets;
using TuiClient.Windows;

namespace TuiClient;

public sealed class TerminalApplication(IApplication app, TargetCollection targets, State state, WindowManager windowManager)
{
	private TargetCollection _targets = targets;
	private State _state = state;
	private WindowManager _windowManager = windowManager;

	public async Task RunAsync()
	{
		// Set login window to be the active window
		app.Run<GuiBootstrapper>();
	}

	public Task Stop()
	{
		app.RequestStop();
		return Task.CompletedTask;
	}

	public Task Dispose()
	{
		_state.Net.Socket?.Close();
		app.Dispose();
		return Task.CompletedTask;
	}
}

public static class TuiServiceCollectionExtensions
{
	public static IServiceCollection AddTui(this IServiceCollection services)
	{
		return services.AddSingleton<IApplication>(_ => Application.Create().Init(DriverRegistry.Names.DOTNET))
			.AddSingleton<WindowManager>()
			.AddSingleton<TerminalApplication>()
			.AddSingleton<State>()

			// Windows
			.AddTransient<FileUnlockWindow>()
			.AddTransient<ConnectingWindow>()
			.AddTransient<SigninWindow>()
			;
	}
}

public sealed class GuiBootstrapper : Runnable
{
	public static GuiBootstrapper Root { get; private set; }

	public GuiBootstrapper()
	{
		Title = "Echo Chat - Dev Client";
	}

	public override void EndInit()
	{
		Root = this;

		base.EndInit();

		Program.Services.GetRequiredService<WindowManager>()
			.Show<FileUnlockWindow>(this);
	}
}