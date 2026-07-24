using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EchoLib.Client;
using EchoLib.Crypto;
using EchoLib.Models;
using EchoLib.Models.Misc;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.ViewModels.Login;

[TransientModel]
public partial class UnlockFileViewModel : ViewModelBase
{
	private readonly ILogger<UnlockFileViewModel> _logger;
	private readonly AppState _state;

	public UnlockFileViewModel(ILogger<UnlockFileViewModel> logger, AppState state)
	{
		_logger = logger;
		_state = state;

		_logger.LogDebug("Initializing new UnlockFileView");
	}

#if DEBUG
	public UnlockFileViewModel(): this(null!, new AppState()) {}
#endif

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
		JUserFile userFile;

		if (IsNewFile)
		{
			userFile = new JUserFile
			{
				Keys = KdvHelper.Generate(),
				Server = new ServerInfoJm
				{
					Address = ServerAddress,
					Port = Port,
					Version = Version
				}
			};
			UserFileHelper.Encrypt(userFile, UserFile, Password);
		}
		else
		{
			// Handle unlocking of existing file
			if (!UserFileHelper.Decrypt(UserFile, Password, out userFile!))
			{
				_logger.LogError("File unlock failed!");
				return;
			}
		}

		// Set userfile and request socket connection through state
		_state.Local.UserFile = userFile;
		_state.Local.AuthState = LocalState.AuthStates.StartConnect;
	}
}