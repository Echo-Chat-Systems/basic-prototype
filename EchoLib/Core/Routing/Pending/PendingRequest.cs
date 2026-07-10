using System.Text.Json;

namespace EchoLib.Core.Routing.Pending;

public sealed class PendingRequest<T> : IPendingRequest
{
	private readonly TaskCompletionSource<T> _source = new();

	public Task<T> Task => _source.Task;

	public void Complete(JsonElement json)
	{
		_source.SetResult(json.Deserialize<T>()!);
	}
}