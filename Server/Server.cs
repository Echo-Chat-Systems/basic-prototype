using EchoLib.Configuration;
using EchoLib.Core;
using EchoLib.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Server.Database;
using Server.Database.ParameterConverters;
using Server.Database.Repositories;
using Server.Database.Repositories.Impl;
using WebSocketSharper.Server;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Server;

public class Server
{
	private WebSocketServer Socket { get; set; } = null!;

	private IServiceProvider Services { get; init; }
	private ILogger<Server> Logger { get; init; }

	public Server()
	{
		// Read in config and .env files
		IConfiguration iConfiguration = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddJsonFile("appsettings.json")
			.AddIniFile("secrets.ini")
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
		services.AddSingleton<ClientManager>();
		services.AddRouting();

		// Database info
		Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
		Dapper.SqlMapper.AddTypeHandler(new PublicSigningKeyConverter());
		services.AddScoped<IDbConnectionProvider, PgDbConnectionProvider>();
		services.AddScoped<IUsersRepo, PgUsersRepo>();

		Services = services.BuildServiceProvider();

		// Get a logger for the main server
		Logger = Services.GetRequiredService<ILogger<Server>>();

		// Configure newtonsoft
		JsonConvert.DefaultSettings = NewtonsoftJson.DefaultSettings;
	}


	public async Task Run()
	{
		Logger.LogInformation("Bootstrapping server...");
		// Get config from services
		Config config = Services.GetService<Config>() ?? throw new Exception("Missing config!");

		// Initialise websocket
		Logger.LogDebug("Creating websocket at address ws{Secure}://{SocketHost}:{SocketPort}", config.Socket.UsingWss ? 's' : "", config.Socket.Host, config.Socket.Port);

		Socket = new WebSocketServer(Services.GetRequiredService<ILogger<WebSocketServer>>(),
			$"ws{(config.Socket.UsingWss ? 's' : "")}://{config.Socket.Host}:{config.Socket.Port}"
		);

		// Get router from services just to initialise it before we spin up anything else
		Services.GetRequiredService<Router>();

		try
		{
			// Register server as websocket service
			Logger.LogDebug("Registering root service.");
			await Socket.AddWebSocketServiceTaskAsync("/", () => new LiveClient(Services));
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		// Start the socket
		Logger.LogInformation("Opening WebSocket.");
		Socket.Start();
		Console.WriteLine("Server is running. Press any key to stop...");
		Console.ReadKey();

		// Stop when called for
		Socket.Stop();
	}
}