using System.Threading.Tasks;
using AvaloniaClient.Dialog;
using AuthOverlayViewModel = AvaloniaClient.ViewModels.Dialog.Login.AuthOverlayViewModel;

namespace AvaloniaClient;

public class AppStartup(
	AppState state,
	DialogService dialogs)
{
	public async Task RunAsync()
	{
		bool result =
			await dialogs.ShowAsync<AuthOverlayViewModel, bool>();

		if (result != true)
		{
			// Application startup failed/cancelled.
			return;
		}



		// At this point AppState is FrontendReady.
		// Continue whatever startup work is required.
	}
}