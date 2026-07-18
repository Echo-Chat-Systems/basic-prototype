using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace TuiClient.Windows;

public class WindowManager : Runnable
{
	private readonly IApplication _app;
	private View? _current;

	public WindowManager(IApplication app)
	{
		_app = app;
	}

	public void Show(View view)
	{
		if (_current != null)
		{
			_app.TopRunnableView?.Remove(_current);
		}

		_current = view;

		_app.TopRunnableView?.Add(view);
		view.SetFocus();
	}
}