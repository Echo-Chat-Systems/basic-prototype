using EchoLib.Client;
using EchoLib.Protocol.Models.Data.User;
using Terminal.Gui.Views;
using TuiClient.Views;

namespace TuiClient.Windows;

public sealed class FileUnlockWindow : Window
{
	private static readonly DirectoryInfo EchoDirectory =  new(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.echo/");
	private readonly ApplicationState _state;

	public FileUnlockWindow(WindowManager windows, ApplicationState state)
	{
		_state = state;

		Title = "Login";

		if (!EchoDirectory.Exists)
		{
			EchoDirectory.Create();
			EchoDirectory.Attributes |= FileAttributes.Hidden;
		}

		// Check if echo file found
		FileInfo userFileHandle = new(EchoDirectory + ".user");


		#region Controls
		StackView stack = new(StackView.Direction.TopToBottom);

		Label passwordLabel = new();
		passwordLabel.Text = "Encryption Password";

		TextField password = new();
		password.Secret = true;

		if (!userFileHandle.Exists)
			// Show controls for creating a new user file
		{

		}

		Button submit = new();
		submit.Text = "Submit";

		stack.AddControl(passwordLabel);
		stack.AddControl(submit);

		#endregion

	}
}

