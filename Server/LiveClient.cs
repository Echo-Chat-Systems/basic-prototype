using System.Runtime.CompilerServices;
using EchoLib.Core.Routing;
using EchoLib.Models.Params.Generic;
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

	public LiveClient(IServiceProvider services)
	{
		// Initialise client state
		_services = services;
		_logger = services.GetRequiredService<ILogger<LiveClient>>();
	}

	protected override void OnMessage(MessageEventArgs e)
	{
		// Build a new context
		RoutingContext ctx = new(Context) { Services = _services };

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
		_ = _services.GetRequiredService<Router>().RouteAsync(ctx, envelope);
		return;

		Fail:
		_logger.LogError("Invalid envelope received!");
	}
}