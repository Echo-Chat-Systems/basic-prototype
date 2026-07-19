using EchoLib.Client;
using EchoLib.Transport;
using Microsoft.Extensions.DependencyInjection;
using WebSocketSharper;

namespace TuiClient;

public class ApplicationState(IServiceProvider services)
{
	private IServiceProvider _services = services;

	public IMessageEndpoint? Endpoint { get; private set; } = null!;
	public WebSocket? Socket { get; private set; }

	public void RegisterConnection(WebSocket socket)
	{
		Socket = socket;
		Endpoint = new WebsocketEndpoint(socket, _services);
	}
}