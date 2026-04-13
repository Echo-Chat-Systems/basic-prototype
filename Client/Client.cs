using Client.Routing;
using EchoLib.Core;
using EchoLib.Core.Routing;
using EchoLib.Models.Crypto;
using EchoLib.Models.Misc;
using EchoLib.Models.Params.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebSocketSharper;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Client;

public class Client
{
	public static readonly DirectoryInfo EchoDirectory = new(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.echo/");
	public WebSocket Socket { get; private set; }

	public static IServiceProvider Services { get; set; } = null!;

	public static KeySetJm Keys { get; set; } = null!;
	public static ServerInfoJm ServerInfo { get; set; } = null!;

	private readonly ILogger<Client> _logger;

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
		services.AddSingleton<SessionInfo>();
		services.AddSingleton<Router>();

		Services = services.BuildServiceProvider();

		// Get the logger 
		_logger = Services.GetService<ILogger<Client>>()!;

		// Configure newtonsoft
		JsonConvert.DefaultSettings = NewtonsoftJson.DefaultSettings;

		// Give UserFile a logger
		UserFile.Logger = Services.GetService<ILogger<UserFile>>();

		// Ensure .echo directory exists 
		if (!EchoDirectory.Exists)
		{
			_logger.LogWarning("Echo Directory {EchoDirectory} did not exist, creating", EchoDirectory);
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
			_logger.LogDebug("User file found at {UserFileHandle}", userFileHandle.Name);
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

			_logger.LogInformation("User file saved to {UserFileLocation}", userFileHandle.FullName);
		}

		// We no longer need the password as plaintext in memory, overwrite
		// ReSharper disable once RedundantAssignment
		passwd = "";

		Keys = file.Keys;
		ServerInfo = file.Server;

		Socket = new WebSocket(Services.GetRequiredService<ILogger<WebSocket>>(),
			$"ws://{ServerInfo.Address}:{ServerInfo.Port}",
			false
		);

		Socket.OnMessage += OnMessage;
		Socket.OnOpen += OnOpen;
	}

	public void Run()
	{
		// Connect
		Socket.Connect();

		//
		string? input;
		while (true)
		{
			Console.Write("Send message: ");
			input = Console.ReadLine();

			if (input is null) continue;

			Socket.Send(input);
		}
	}


	private void OnOpen(object? sender, EventArgs e)
	{
		// Create new ctx
		RoutingContext ctx = new(Socket) { Services = Services };

		// Get router
		Router router = Services.GetRequiredService<Router>();
		
		// Send hello message
		_logger.LogDebug("Sending client-hello");
		router.GetTarget<AuthTarget>()?.SendHello(ctx, new ClientHelloParameters { Id = Keys.PubSk });
	}

	private void OnMessage(object? sender, MessageEventArgs e)
	{
		// Build a new context
		RoutingContext ctx = new(Socket) { Services = Services };

		// Unpack message event 
		_logger.LogDebug("Message received, attempting to unpack");
		MessageEnvelope<object>? envelope = null;
		try
		{
			envelope = JsonConvert.DeserializeObject<MessageEnvelope<object>>(e.Data);
		}
		catch (JsonReaderException)
		{
			goto Fail;
		}

		if (envelope is null) goto Fail;

		_logger.LogDebug("Unpacked message {Target}", envelope.Target);

		// Route message
		_ = Services.GetRequiredService<Router>().RouteAsync(ctx, envelope);
		return;

		Fail:
		_logger.LogError("Invalid envelope received!");
	}

	public class SessionInfo
	{
		public string ServerName { get; set; } = null!;
	}
}