using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaClient.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using EchoLib.Protocol;
using EchoLib.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharper;

namespace AvaloniaClient.Views;

public partial class MainWindow : LoggableWindow
{
	private AppState _state;
	private Router _router;

	public MainWindow()
	{
		_state = Ioc.Default.GetRequiredService<AppState>();
		_router = Ioc.Default.GetRequiredService<Router>();

		InitializeComponent();

		DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
	}
}