using System.Text.Json;
using EchoLib.Core.Routing.Pending;
using EchoLib.Protocol;
using WebSocketSharper;

namespace EchoLib.Core.Routing;

public sealed class Connection(WebSocket socket, Router router)
{
	private readonly WebSocket _socket = socket;
	private readonly Router _router = router;
	private readonly PendingRequests _pending = new();


	public async Task<T> SendAsync<T>(
		string target,
		string action,
		object parameters)
	{
		Guid id =
			Guid.NewGuid();

		Task<T> task =
			_pending.Register<T>(id);

		await Send(
			new Envelope
			{
				MessageId = id,
				Target = target,
				Data = new MessageData
				{
					Action = action,
					Parameters =
						JsonSerializer.SerializeToElement(parameters)
				}
			});


		return await task;
	}


	private Task Send(Envelope message)
	{
		// websocket serialization here
		throw new NotImplementedException();
	}
}