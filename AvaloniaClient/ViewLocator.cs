using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaClient.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace AvaloniaClient;

/// <summary>
/// Given a view model, returns the corresponding view if possible.
/// </summary>
[RequiresUnreferencedCode(
	"Default implementation of ViewLocator involves reflection which may be trimmed away.",
	Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate
{
	public Control? Build(object? param)
	{
		if (param is null)
			return null;

		string name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
		Type? type = Type.GetType(name);

		if (type != null)
		{
			Control? view = (Control?)Activator.CreateInstance(type);
			view?.DataContext = Ioc.Default.GetService(param.GetType());

			return view;
		}

		return new TextBlock { Text = "Not Found: " + name };
	}

	public bool Match(object? data)
	{
		return data is ViewModelBase;
	}
}