using System;
using System.Threading.Tasks;
using AvaloniaClient.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaClient.Dialog;

[SingletonModel]
public partial class DialogService : ObservableObject, IDialogService
{
	private readonly IServiceProvider _services;

	[ObservableProperty] public partial object? CurrentDialog { get; private set; }

	[ObservableProperty] public partial bool IsOpen { get; private set; }

	private TaskCompletionSource<object?>? _completion;

	public DialogService(IServiceProvider services)
	{
		_services = services;
	}

	public async Task<TResult?> ShowAsync<TViewModel, TResult>()
		where TViewModel : ViewModelBase
	{
		if (_completion != null)
			throw new InvalidOperationException("A dialog is already open.");

		TViewModel viewModel = _services.GetRequiredService<TViewModel>();

		_completion = new TaskCompletionSource<object?>(
			TaskCreationOptions.RunContinuationsAsynchronously);

		CurrentDialog = viewModel;
		IsOpen = true;

		object? result;

		try
		{
			result = await _completion.Task;
		}
		finally
		{
			CurrentDialog = null;
			IsOpen = false;
			_completion = null;
		}

		return (TResult?)result;
	}

	public void Close<TResult>(TResult result)
	{
		_completion?.TrySetResult(result);
	}

	public void Cancel()
	{
		_completion?.TrySetResult(null);
	}
}