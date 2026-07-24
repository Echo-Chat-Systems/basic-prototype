using System;
using System.ComponentModel.Design;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
			.AddSingleton<AuthManager>()
			.BuildServiceProvider();

		Ioc.Default.ConfigureServices(services);

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow();
		}

		base.OnFrameworkInitializationCompleted();
	}
}