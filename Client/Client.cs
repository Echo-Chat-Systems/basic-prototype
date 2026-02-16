using System.Security.AccessControl;
using EchoLib.Core;
using EchoLib.Core.Routing;
using EchoLib.Models.Crypto;
using EchoLib.Models.Misc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebSocketSharp;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Client;

public class Client
{
	public static readonly DirectoryInfo EchoDirectory = new(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.echo/");
	public WebSocket Socket { get; private set; }
	public static IServiceProvider Services { get; set; }
	private static KeySetJm Keys { get; set; }
	private static ServerInfoJm ServerInfo { get; set; }

	private static Router _router; 

	public Client()
	{
		Console.Clear();
		// Configure logger

		// Build service collection
		ServiceCollection services = new();
		services.AddLogging(builder =>
		{
			builder.AddConsole();
			builder.SetMinimumLevel(LogLevel.Debug);
		});

		Services = services.BuildServiceProvider();

		// Get the logger 
		ILogger<Client> logger = Services.GetService<ILogger<Client>>()!;

		// Give UserFile a logger
		UserFile.Logger = Services.GetService<ILogger<UserFile>>();

		// Ensure .echo directory exists 
		if (!EchoDirectory.Exists)
		{
			logger.LogWarning("Echo Directory {EchoDirectory} did not exist, creating", EchoDirectory);
			EchoDirectory.Create();

			// Set directory to hidden
			EchoDirectory.Attributes |= FileAttributes.Hidden;
		}

		// Ask user for their password
		string passwd = ConsoleHelper.GetConsoleInput("Please input your encryption password: ");
		FileInfo userFileHandle = new(EchoDirectory + ".user");
		UserFile file;


		// Check if file exists
		if (userFileHandle.Exists)
		{
			logger.LogDebug("User file found at {UserFileHandle}", userFileHandle.Name);
			// Attempt to read the user file
			if (!UserFile.Decrypt(userFileHandle, passwd, out file) || file == null)
			{
				Console.WriteLine("Invalid password, exiting");
				throw new Exception("Invalid password"); // This is really hacky
			}
		}
		// User does not have an existing saved account, create a new account
		else
		{
			file = new UserFile
			{
				Keys = KdvHelper.Generate(),
				Server = new ServerInfoJm
				{
					Address = ConsoleHelper.GetConsoleInput("Input server address: "),
					Port = int.Parse(ConsoleHelper.GetConsoleInput("Input server port: ")),
					Version = ConsoleHelper.GetConsoleInput("Input server version (leave blank for unknown): ")
				}
			};

			// Save file
			UserFile.Encrypt(file, userFileHandle, passwd);

			logger.LogInformation("User file saved to {UserFileLocation}", userFileHandle.FullName);
		}

		Keys = file.Keys;
		ServerInfo = file.Server;

		Socket = new WebSocket($"ws://{ServerInfo.Address}:{ServerInfo.Port}");

		// Build the router
		_router = new Router(new RoutingContext
		{
			Services = Services,
			Socket = Socket
		});
		
		Socket.OnMessage += OnMessage;
	}

	public void Run()
	{
		// Connect
		Socket.Connect();
		
		// Begin connect procedure
		

		string? input;
		while (true)
		{
			Console.Write("Send message: ");
			input = Console.ReadLine();

			if (input is null) continue;

			Socket.Send(input);
		}
	}


	private void OnMessage(object? sender, MessageEventArgs e)
	{
		// Parse the message as JSON
		
	}
}