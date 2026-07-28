using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaClient.Dialog;
using AvaloniaClient.Managers;
using AvaloniaClient.ViewModels;
using AvaloniaClient.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using EchoLib.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient;

public partial class App : Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		// Register services
		IServiceProvider services = new ServiceCollection()
			.AddLogging(l =>
			{
				l.AddConsole();
				l.SetMinimumLevel(LogLevel.Debug);
			})
			.AddRouting()
			.AddViewModels()
			.AddSingleton<AppState>()
			.AddSingleton<AppStartup>()
			.AddSingleton<AuthManager>()
			.AddSingleton<DialogService>()
			.BuildServiceProvider();

		Ioc.Default.ConfigureServices(services);

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			MainWindow mainWindow = new();
			MainViewModel mainViewModel = services.GetRequiredService<MainViewModel>();

			mainWindow.DataContext = mainViewModel;

			desktop.MainWindow = mainWindow;

			mainWindow.Show();

			_ = RunStartupAsync();

		}

		base.OnFrameworkInitializationCompleted();
	}

	private async Task RunStartupAsync()
	{
		try
		{
			await Ioc.Default
				.GetRequiredService<AppStartup>()
				.RunAsync();
		}
		catch (Exception ex)
		{
			// Log startup failure.
		}
	}
}