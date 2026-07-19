using System.IO;
using System.Windows;
using System.Windows.Input;
using EchoLib.Configuration;
using EchoLib.Core;
using EchoLib.Crypto;
using EchoLib.Models.Misc;
using EchoLib.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebSocketSharper;

namespace GuiClient.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class StartupWindow : Window
{
	private readonly DirectoryInfo _echoDir;
	private IServiceProvider Services { get; init; }
	private ILogger<StartupWindow> Logger { get; init; }

	private UserFile? UserFile = null;
	
	public StartupWindow()
	{
		InitializeComponent();
		
		// Get application config
		Config config = ConfigBuilder.Build<Config>(
			new ConfigurationBuilder()
				.AddEnvironmentVariables()
				.AddJsonFile("appsettings.json")
				.Build()
		);
		
		// Build Service collection 
		ServiceCollection services = new();

		services.AddLogging(builder =>
		{
			builder.AddConsole();
			builder.SetMinimumLevel(LogLevel.Debug);
		});

		services.AddSingleton(config);
		services.AddSingleton<Router>();
		services.AddSingleton<StateStore>();

		Services = services.BuildServiceProvider();
		
		// Get Logger
		Logger = Services.GetRequiredService<ILogger<StartupWindow>>();
		
		// Configure newtonsoft 
		JsonConvert.DefaultSettings = NewtonsoftJson.DefaultSettings;
		
		// Get user file path
		_echoDir = config.Persistence.EchoDirectory == "" ?
			new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.echo/") 
			: new DirectoryInfo(config.Persistence.EchoDirectory);
		
		// Ensure directory exists
		if (!_echoDir.Exists)
		{
			Logger.LogWarning("Echo {Directory} did not exist, creating", _echoDir.Name);
			_echoDir.Create();
		}
		
		// Check if file exists
		FileInfo file = new(_echoDir + ".user");

		// Show new account panels if not created
		if (!file.Exists) NewAccountPanels.Visibility = Visibility.Visible;
		
		// Add listener for enter event
	}

	private void Login()
	{
		// First ensure that there is content in the EncryptionPassword 
		if (EncryptionPasswordInput.Password == "")
		{
			DialogHelpers.ShowError(this, "Encryption Password must be set");
			return;
		}

		FileInfo userFile = new FileInfo(_echoDir + ".user");
		
		if (!userFile.Exists)
		{
			// Ensure that the address and port contain at least something
			if (ServerAddressInput.Text == "" || ServerPortInput.Text == "")
			{
				DialogHelpers.ShowError(this, "Server Address and Port are mandatory!");
				return;
			}
			
			// Convert port into a number 
			if (!int.TryParse(ServerPortInput.Text, out int port))
			{
				DialogHelpers.ShowError(this, "Port must be a valid number");
				return;
			}
			
			// Ensure port is valid 
			if (port is < 1 or > ushort.MaxValue)
			{
				DialogHelpers.ShowError(this, $"Port must be between 1 and {ushort.MaxValue}");
				return;
			}
			
			// Create a new userFile
			UserFile = new UserFile
			{
				Keys = KdvHelper.Generate(),
				Server = new ServerInfoJm
				{
					Address = ServerAddressInput.Text,
					Port = port,
					Version = ServerVersionInput.Text
				}
			};
			
			// Save file 
			UserFile.Encrypt(UserFile, userFile, EncryptionPasswordInput.Password);
			
			Logger.LogInformation("User file saved to {UserFileName}", userFile.Name);
		}

		else
		{
			Logger.LogDebug("User file found at {Path}", userFile.Name);
			
			// Attempt to decrypt user file 
			if (!UserFile.Decrypt(userFile, EncryptionPasswordInput.Password, out UserFile) || UserFile == null)
			{
				DialogHelpers.ShowError(this, "Invalid encryption password.");
				return;
			}
		}
		
		// We now have a valid user file.
		StateStore state = Services.GetRequiredService<StateStore>();

		state.UserFile = UserFile;
		state.Socket = new WebSocket(Services.GetRequiredService<ILogger<WebSocket>>(),
			$"ws://{UserFile.Server.Address}:{UserFile.Server.Port}",
			false);
		
		// Spin up new instance of ClientWindow
		ClientWindow client = new(this, Services);
	}
	
	protected override void OnKeyDown(KeyEventArgs e)
	{
		// Check if the key pressed is enter
		if (e.Key == Key.Enter)
		{
			Login();
			return;
		}
		
		base.OnKeyDown(e);
	}

	private void LoginButton_OnClick(object sender, RoutedEventArgs e) => Login();
}