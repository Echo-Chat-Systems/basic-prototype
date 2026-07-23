using EchoLib.Transport;
using WebSocketSharper;

namespace TuiClient;

public class NetState(IServiceProvider services)
{
	public IMessageEndpoint? Endpoint { get; private set; } = null!;
	public WebSocket? Socket { get; private set; }



	public void RegisterConnection(WebSocket socket)
	{
		Socket = socket;
		Endpoint = new WebsocketEndpoint(socket, services);
	}
}