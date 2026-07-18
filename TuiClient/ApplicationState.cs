using EchoLib.Client;
using EchoLib.Transport;
using Microsoft.Extensions.DependencyInjection;
using WebSocketSharper;

namespace TuiClient;

public class ApplicationState(IServiceProvider services)
{
	public readonly IMessageEndpoint Endpoint = new WebsocketEndpoint(services.GetRequiredService<WebSocket>(), services);
	public UserFile UserFile { get; set; } = null!;
}