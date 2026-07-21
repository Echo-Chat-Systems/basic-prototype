using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace TuiClient.Windows;

public class WindowManager
{
	private readonly IApplication _app;
	private readonly IServiceProvider _services;
	public View? Current { get; private set; }

	public WindowManager(IServiceProvider services, IApplication app)
	{
		_services = services;
		_app = app;
	}

	public void Show<T>(Runnable root) where T : View
	{
		// Create a new instance of the view with DI and show
		Show(_services.GetRequiredService<T>(), root);
	}

	public void Show(View view, Runnable parent)
	{
		/*if (Current != null)
		{
			_app.TopRunnableView?.Remove(Current);
		}

		Current = view;

		view.Width = Dim.Fill();
		view.Height = Dim.Fill();

		_app.TopRunnableView?.Add(view);

		view.SetFocus();
		*/
		if (Current != null)
		{
			parent.Title.Remove(parent.Title.IndexOf($" - {Current.Title}", StringComparison.Ordinal));
			parent.Remove(Current);
		}


		parent.Add(view);
		view.Width = Dim.Fill();
		view.Height = Dim.Fill();
		view.SetFocus();

		// Set title
		parent.Title = $"{parent.Title} - {view.Title}";

		Current = view;
	}
}