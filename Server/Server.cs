using EchoLib.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Configuration;
using WebSocketSharp;
using WebSocketSharp.Server;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Server;

public class Server : WebSocketBehavior
{
	private WebSocketServer Socket { get; set; } = null!;
	
	private ServiceProvider Services { get; init; }
	private ILogger<Server> _logger { get; init; }

	public Server()
	{
		// Read in config and .env files
		IConfiguration iConfiguration = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddJsonFile("appsettings.json")
			.Build();
		
		// Use config library to build config class 
		Config config = ConfigBuilder.Build<Config>(iConfiguration);
		
		// Build service collection
		ServiceCollection services = new();
		services.AddLogging(builder =>
		{
			builder.AddConsole();
			builder.SetMinimumLevel(LogLevel.Debug);
		});
		services.AddSingleton(config);

		Services = services.BuildServiceProvider();
		
		// Get a logger for the main server
		_logger = Services.GetService<ILogger<Server>>()!;
	}

	
	public async void Run()
	{
		_logger.LogInformation("Bootstrapping server...");
		// Get config from services
		Config config = Services.GetService<Config>() ?? throw new Exception("Missing config!");
		
		// Initialise websocket
		_logger.LogInformation("Opening WebSocket.");
		Socket = new WebSocketServer($"ws{(config.Socket.UsingWss ? 's' : "")}://{config.Socket.Host}:{config.Socket.Port}");
		
		// Register server as websocket service
		Socket.AddWebSocketService<Server>("/");
		
		// Start the socket
		Socket.Start();
		Console.WriteLine("Server is running. Press any key to stop...");
		Console.ReadKey();
		
		// Stop when called for
		Socket.Stop();
	}
	
	/// <summary>
	/// Handle incoming messages by parsing them into a message envelope and handing off to router.
	/// </summary>
	/// <param name="e">
	///	Message arguments.
	/// </param>
	protected override void OnMessage(MessageEventArgs e)
	{
		_logger.LogDebug("Received message: {EData}", e.Data);
		
		Sessions.Broadcast(e.Data);
	}
	
}