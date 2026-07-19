using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace TuiClient.Windows;

public class WindowManager
{
	private readonly IApplication _app;
	private readonly IServiceProvider _services;
	public Window? Current { get; private set; }

	public WindowManager(IServiceProvider services, IApplication app)
	{
		_services = services;
		_app = app;
	}

	public void Show<T>() where T : Window
	{
		// Create a new instance of the view with DI and show
		Show(_services.GetRequiredService<T>());
	}

	public void Show(Window view)
	{
		if (Current != null)
		{
			_app.TopRunnableView?.Remove(Current);
		}

		Current = view;

		_app.TopRunnableView?.Add(view);
		view.SetFocus();
	}
}