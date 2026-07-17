using System.Runtime.CompilerServices;
using System.Text.Json;
using EchoLib.Protocol;
using EchoLib.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharper;
using WebSocketSharper.Server;

namespace Server;

public class LiveClient : WebSocketBehavior
{
	private IServiceProvider _services;
	private ILogger<LiveClient> _logger;
	private ClientManager _clientManager;
	private Router _router;

	public LiveClient(IServiceProvider services)
	{
		// Initialise client state
		_services = services;
		_router = services.GetRequiredService<Router>();
		_clientManager = _services.GetRequiredService<ClientManager>();
		_logger = services.GetRequiredService<ILogger<LiveClient>>();
	}

	protected override void OnOpen()
	{
		_logger.LogDebug("New client connected. Registering now");
		_clientManager.Register(this);
	}

	protected override void OnMessage(MessageEventArgs e)
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
		_router.Receive(envelope, Context.WebSocket);
		return;

		Fail:
		_logger.LogError("Invalid envelope received!");
	}
}