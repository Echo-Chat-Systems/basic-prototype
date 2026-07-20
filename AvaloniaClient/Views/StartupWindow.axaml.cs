using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaClient.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.Views;

public partial class StartupWindow : Window
{
	private ILogger<StartupWindow> _logger;
	public StartupWindow()
	{
		InitializeComponent();
		_logger = Ioc.Default.GetRequiredService<ILogger<StartupWindow>>();
	}

	private async void Login_OnClick(object? sender, RoutedEventArgs e)
	{
		try
		{
			UnlockIdFileWindow window = new();
			window.DataContext = new UnlockIdFileViewModel(window);
			await window.ShowDialog<bool?>(this);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while logging in.");
		}
	}
}