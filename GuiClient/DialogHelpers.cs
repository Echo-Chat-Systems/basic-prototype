using System.Windows;

namespace GuiClient;

public static class DialogHelpers
{
	public static void ShowError(Window owner, string message)
	{
		MessageBox.Show(owner, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
	}
}