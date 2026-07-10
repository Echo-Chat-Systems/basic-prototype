using System.Collections.Concurrent;
using System.Text.Json;

namespace EchoLib.Core.Routing.Pending;

public sealed class PendingRequests
{
	private readonly ConcurrentDictionary<Guid, IPendingRequest> _pending = new();

	public Task<T> Register<T>(Guid id)
	{
		PendingRequest<T> request = new();

		_pending[id] = request;

		return request.Task;
	}

	public void Complete(Guid id, JsonElement payload)
	{
		if (_pending.TryRemove(id, out IPendingRequest request)) request.Complete(payload);
	}
}