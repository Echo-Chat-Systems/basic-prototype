using Avalonia.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.Views;

public abstract class LoggableWindow : Window
{
	public ILogger<LoggableWindow> Logger { get; } = Ioc.Default.GetRequiredService<ILogger<LoggableWindow>>();
}