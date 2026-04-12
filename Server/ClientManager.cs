using EchoLib.Core.Crypto.Signing;
using WebSocketSharper.Server;

namespace Server;

public class ClientManager
{
	private readonly List<ServerClient> _clients = new();

	public class ServerClient
	{
		public required LiveClient Instance;
		public bool LoggedIn = false;
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
}