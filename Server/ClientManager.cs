using EchoLib.Core.Crypto.Signing;
using WebSocketSharper;
using WebSocketSharper.Server;

namespace Server;

public class ClientManager
{
	private readonly List<ServerClient> _clients = new();

	public class ServerClient
	{
		public required LiveClient Instance;
		public bool Authenticated = false;
		public PublicSigningKey? Id;
	}

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