using EchoLib.Core.Crypto.Signing;
using EchoLib.Models.States;
using Server.Routing;
using WebSocketSharper;
using WebSocketSharper.Server;

namespace Server;


public class ServerClient
{
	public required LiveClient Instance;
	public PublicSigningKey? Id;

	public AuthTarget.SigninState SigninState = new();
}

public class ClientManager
{
	private readonly List<ServerClient> _clients = new();

	

	public Task Register(LiveClient client)
	{
		_clients.Add(new ServerClient
		{
			Instance = client
		});

		return Task.CompletedTask;
	}

	/// <summary>
	/// Find a client from the manager.
	/// </summary>
	/// <param name="client"></param>
	/// <returns></returns>
	public ServerClient? Get(LiveClient client)
	{
		return _clients.Find(c => c.Instance == client);
	}

	public ServerClient? Get(WebSocket sock)
	{
		return _clients.Find(c => c.Instance.Context.WebSocket == sock);
	}
}