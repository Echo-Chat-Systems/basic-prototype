using System.Runtime.CompilerServices;
using EchoLib.Core.Routing;
using EchoLib.Protocol;
using EchoLib.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebSocketSharper;
using WebSocketSharper.Server;

namespace Server;

public class LiveClient : WebSocketBehavior
{
	private IServiceProvider _services;
	private ILogger<LiveClient> _logger;
	private Router  _router;

	public LiveClient(IServiceProvider services)
	{
		// Initialise client state
		_services = services;
		_router = _services.GetRequiredService<Router>();
		_logger = services.GetRequiredService<ILogger<LiveClient>>();
	}

	protected override void OnOpen()
	{
		_logger.LogDebug("New client connected. Registering now");

		ClientManager manager = _services.GetRequiredService<ClientManager>();

		_ = manager.Register(this);
	}

	protected override void OnMessage(MessageEventArgs e)
	{
		// Unpack message event 
		_logger.LogDebug("Message received, attempting to unpack");
		Envelope? envelope; 
		try
		{
			envelope = JsonConvert.DeserializeObject<Envelope>(e.Data);
		}
		catch (JsonReaderException)
		{
			goto Fail;
		}

		if (envelope is null) goto Fail;

		_logger.LogDebug("Unpacked message {Target}", envelope.Target);

		// Route message
		_router.Receive(envelope);
		return;

		Fail:
		_logger.LogError("Invalid envelope received!");
	}
}