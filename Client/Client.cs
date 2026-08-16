using Client.Routing;
using EchoLib.Client;
using EchoLib.Core;
using EchoLib.Crypto;
using EchoLib.Models;
using EchoLib.Models.Crypto;
using EchoLib.Models.Misc;
using EchoLib.Models.Params.Auth;
using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing;
using EchoLib.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharper;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Client;

public class Client
{
	public static readonly DirectoryInfo EchoDirectory =
		new(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.echo/");

	public static WebSocket Socket { get; private set; }
	public static IServiceProvider Services { get; set; } = null!;
	public static JKeySet Keys { get; set; } = null!;
	public static ServerInfoJm ServerInfo { get; set; } = null!;
	private Router router;
	private bool _connected = false;

	private readonly ILogger<Client> _logger;
	private readonly Targets _targets;

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
		services.AddSingleton<Targets>();
		services.AddRouting();

		Services = services.BuildServiceProvider();

		// Get the logger 
		_logger = Services.GetService<ILogger<Client>>()!;

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
		JUserFile file;


		// Check if file exists
		if (userFileHandle.Exists)
		{
			_logger.LogDebug("User file found at {UserFileHandle}", userFileHandle.Name);
			// Attempt to read the user file
			if (!UserFileHelper.Decrypt(userFileHandle, passwd, out file) || file == null)
			{
				Console.WriteLine("Invalid password, exiting");
				throw new Exception("Invalid password"); // This is really hacky
			}
		}
		// User does not have an existing saved account, create a new account
		else
		{
			file = new JUserFile
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
			UserFileHelper.Encrypt(file, userFileHandle, passwd);

			_logger.LogInformation("User file saved to {UserFileLocation}", userFileHandle.FullName);
		}

		// We no longer need the password as plaintext in memory, overwrite
		// ReSharper disable once RedundantAssignment
		passwd = "";

		Keys = file.Keys;
		ServerInfo = file.Server;

		router = Services.GetRequiredService<Router>();
		_targets = Services.GetRequiredService<Targets>();

		Socket = new WebSocket(Services.GetRequiredService<ILogger<WebSocket>>(),
			$"ws://{ServerInfo.Address}:{ServerInfo.Port}",
			false
		);

		Socket.OnMessage += OnMessage;
		Socket.OnOpen += OnOpen;
	}

	public async Task Run()
	{
		// Connect
		Socket.ConnectAsync();

		// Wait until the server is connected
		while (!_connected)
		{
		}

		// Connect to server
		await _targets.Auth.SendHello(new WebsocketEndpoint(Socket, Services), new ClientHelloParameters
		{
			KeyPair = new JPublicKeyPair
			{
				SigningKey = Keys.PubSk, EncryptionKey = Keys.PubEk
			}
		});

		try
		{
			await _targets.Auth.SendSigninStart(new WebsocketEndpoint(Socket, Services), new SigninStartParameters
			{
				Sk = Keys.PubSk,
				Ek = Keys.PubEk
			});
		}
		catch (NotFoundException)
		{
			// Try and create an account on the server
			_logger.LogError("No account of Id {Id} reported by server", Keys.PubSk);
			//TODO: Put signup call here
		}


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
		// Send hello message
		_logger.LogDebug("Socket connected.");

		_connected = true;
	}

	private void OnMessage(object? sender, MessageEventArgs e)
	{
		// Unpack message event 
		_logger.LogDebug("Message received, attempting to unpack");
		Envelope<JToken>? envelope;
		try
		{
			envelope = JsonConvert.DeserializeObject<Envelope<JToken>>(e.Data);
		}
		catch (JsonReaderException)
		{
			goto Fail;
		}

		if (envelope is null) goto Fail;

		_logger.LogDebug("Unpacked message {Target}", envelope.Target);

		// Route message
		Services.GetRequiredService<Router>().Receive(envelope, Socket);
		return;

		Fail:
		_logger.LogError("Invalid envelope received!");
	}

	public class SessionInfo
	{
		public string ServerName { get; set; } = null!;
	}
}