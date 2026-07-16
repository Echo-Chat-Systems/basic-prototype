using System.Collections.Concurrent;
using EchoLib.Protocol;

namespace EchoLib.Routing.Responses;

public class PendingResponseRegistry
{
    private ConcurrentDictionary<Guid, IPendingRequest> _pending = new();
    
    public bool TryRemove(Guid? messageId, out IPendingRequest? response)
    {
        response = null;
        
        // Check if this is a registered reply being waited for
        if (messageId == null || !_pending.ContainsKey((Guid)messageId)) return false;

        _pending.TryRemove((Guid)messageId, out response);
        return true;
    }

    public Task<TResponse> Register<TResponse>(Guid messageId)
    {
	    PendingRequest<TResponse> pending = new(messageId);

	    if (!_pending.TryAdd(messageId, pending)) throw new InvalidOperationException($"Existing message with {messageId} already exists in pending");

	    return pending.Task;
    }
}