using System.Threading.Tasks;
using AvaloniaClient.ViewModels;

namespace AvaloniaClient.Dialog;

public interface IDialogService
{
	Task<TResult?> ShowAsync<TViewModel, TResult>()
		where TViewModel : ViewModelBase;

	void Close<TResult>(TResult result);

	void Cancel();
}