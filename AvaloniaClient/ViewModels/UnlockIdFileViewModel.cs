using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using AvaloniaClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EchoLib.Client;
using EchoLib.Crypto;
using EchoLib.Models;
using EchoLib.Models.Misc;

namespace AvaloniaClient.ViewModels;

public partial class UnlockIdFileViewModel : ViewModelBase
{
	private readonly Window? _diag = null;

	public UnlockIdFileViewModel(Window diag)
	{
		_diag = diag;
	}

	public UnlockIdFileViewModel()
	{

	}

	public static FileInfo UserFile =>
		new(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.echo/.user");

	public bool IsNewFile => !UserFile.Exists;

	[ObservableProperty] public partial string Password { get; set; } = "";

	#region New File Properties

	[ObservableProperty] public partial string ServerAddress { get; set; } = "localhost";
	[ObservableProperty] public partial int Port { get; set; } = 6969;
	[ObservableProperty] public partial string Version { get; set; } = "V1";

	#endregion

	public string UnlockButtonContents => IsNewFile ? "Create" : "Unlock";

	[RelayCommand]
	private async Task UnlockAsync()
	{
		State.AppState state = Ioc.Default.GetRequiredService<State.AppState>();

		if (IsNewFile)
		{
			state.UserFile = new JUserFile
			{
				Keys = KdvHelper.Generate(),
				Server = new ServerInfoJm
				{
					Address = ServerAddress,
					Port = Port,
					Version = Version
				}
			};

			UserFileHelper.Encrypt(state.UserFile, UserFile, Password);
		}
		else
		{
			// Handle unlocking of existing file
			if (!UserFileHelper.Decrypt(UserFile, Password, out JUserFile? userFile))
			{
				_diag!.Close(false);
			}

			state.UserFile = userFile;
		}

		_diag!.Close(true);
	}
}