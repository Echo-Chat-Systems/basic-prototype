using EchoLib.Core;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using TuiClient.Targets;
using TuiClient.Windows;

namespace TuiClient;

public sealed class TerminalApplication(IApplication app, TargetCollection targets, ApplicationState state, WindowManager windowManager)
{
	private TargetCollection _targets = targets;
	private ApplicationState _state = state;
	private WindowManager _windowManager = windowManager;

	public Task RunAsync()
	{
		// Set login window to be the active window
		app.Run<GuiBootstrapper>();

		return Task.CompletedTask;
	}

	public Task RunAsync(CancellationToken ct)
	{
		throw new NotImplementedException();
	}

	public Task Stop()
	{
		throw new NotImplementedException();
	}

	public Task Dispose()
	{
		throw new NotImplementedException();
	}
}

public static class TuiServiceCollectionExtensions
{
	public static IServiceCollection AddTui(this IServiceCollection services)
	{
		return services.AddSingleton<IApplication>(_ => Application.Create().Init())
			.AddSingleton<WindowManager>()
			.AddSingleton<TerminalApplication>()
			.AddSingleton<ApplicationState>()

			// Windows
			.AddTransient<FileUnlockWindow>()

			;
	}
}

public sealed class GuiBootstrapper : Runnable
{
	public GuiBootstrapper()
	{

		Title = "why isn't this working";
		Add(new Label
		{
			Text = "Test",
			X = 1,
			Y = 1
		});




		// Program.Services.GetRequiredService<WindowManager>()
		// 	.Show<FileUnlockWindow>();
	}


}