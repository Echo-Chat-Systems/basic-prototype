using System.Text.RegularExpressions;
using EchoLib.Client;
using EchoLib.Models;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.Input;
using Terminal.Gui.Text;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using TuiClient.Components;

namespace TuiClient.Windows;

public sealed class FileUnlockWindow : View
{
	private static readonly DirectoryInfo EchoDirectory = new(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.echo/");
	private static FileInfo EchoFile => new (EchoDirectory + ".user");

	private readonly WindowManager _windows;
	private readonly State _state;

	#region Controls

	private static readonly Label IpLabel = new() { Text = "Server IP" };
	private static readonly TextField IpField = new() { TabStop = TabBehavior.TabStop };

	private static readonly Label PortLabel = new() { Text = "Server Port" };
	private static readonly TextField PortField = new() { TabStop = TabBehavior.TabStop };

	private static readonly Label VersionLabel = new() { Text = "Server Version" };
	private static readonly TextField VersionField = new() { TabStop = TabBehavior.TabStop };

	private static readonly Label PasswordLabel = new() { Text = "Encryption Password" };
	private static readonly TextField PasswordField = new()
	{
		Secret = true,
		TabStop = TabBehavior.TabStop
	};

	private static readonly Label ErrorLabel = new()
	{
		Text = "%error%",
		Visible = false,
	};

	#endregion

	public FileUnlockWindow(WindowManager windows, State state)
	{
		_windows = windows;
		_state = state;

		Title = "Unlock User File";

		CanFocus = true;

		Width = Dim.Fill();
		Height = Dim.Fill();

		if (!EchoDirectory.Exists)
		{
			EchoDirectory.Create();
			EchoDirectory.Attributes |= FileAttributes.Hidden;
		}

		#region Controls

		StackView stack = new(StackView.Direction.TopToBottom)
		{
			TabStop = TabBehavior.TabGroup
		};



		if (!EchoFile.Exists)
			// Show controls for creating a new user file
		{
			IpField.TextChanged += OnIpChanged;
			PortField.TextChanged += OnPortChanged;
			stack.AddControl(IpLabel, IpField, PortLabel, PortField, VersionLabel, VersionField);
		}

		Button submit = new()
		{
			Text = "Submit",
			TabStop = TabBehavior.TabStop,
			HotKey = Key.Enter
		};

		submit.Activating += OnSubmit;

		stack.AddControl(PasswordLabel, PasswordField, submit, ErrorLabel);

		stack.Height = Dim.Fill();
		stack.Width = Dim.Percent(75);

		stack.X = Pos.Center();
		stack.Y = Pos.Center();

		Add(stack);

		#endregion
	}

	private void OnIpChanged(object? o, EventArgs e)
	{
		// For now this will do nothing
		// TODO: Make this validate an IP address / DNS resolvable address automatically
	}

	private static void OnPortChanged(object? o, EventArgs e)
	{
		PortLabel.Text = !ushort.TryParse(PortField.Text, out ushort result) ? "Server Port - *INVALID PORT*" : "Server Port";
	}

	private void OnPasswordChanged(object? o, EventArgs e) => PasswordLabel.Text = "Encryption Password";

	private void OnSubmit(object? o, CommandEventArgs e)
	{
		if (EchoFile.Exists)
		{
			// Try and unlock echo file with key
			if (!UserFileHelper.Decrypt(EchoFile, PasswordField.Text, out UserFileJm? userFile))
			{
				PasswordLabel.Text = "Encryption Password - *Invalid Password!*";
				return;
			}

			// Decryption worked
			Program.Services.GetRequiredService<State>().Local.UserFile = userFile;
			Program.Services.GetRequiredService<WindowManager>().Show<ConnectingWindow>(GuiBootstrapper.Root);
		}
		else
		{
			// Try and convert port to an int
			if (!ushort.TryParse(PortField.Text, out ushort result))
			{
				// Fail and display error message
				PortLabel.Text = "Server Port - *INVALID PORT*";
				return;
			}

			// Create new user file

		}
	}
}