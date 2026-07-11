using System.Text.Json;
using EchoLib.Core.Routing;
using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Protocol.Models.Params.Generic;

namespace EchoLib.Routing.Responses;

public interface IPendingRequest
{
	Guid MessageId { get; }
	Type ResponseType { get; }
	
	void Complete(Envelope msg);
	void Fail(Envelope msg);
}

public sealed class PendingRequest<T> : IPendingRequest
{
	private readonly TaskCompletionSource<T> _tcs;
	
	
	public Guid MessageId { get; }
	public Type ResponseType => typeof(T);
	public Task<T> Task => _tcs.Task;
	public PendingRequest(Guid messageId)
	{
		MessageId = messageId;
		
		_tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
	}
	public void Complete(Envelope msg)
	{
		// Deserialise into T
		T response = msg.Data.Parameters.Deserialize<T>() ?? throw new InvalidOperationException("Could not deserialise parameters");
		
		_tcs.SetResult(response);
	}

	public void Fail(Envelope msg)
	{
		// Deserialise into error params
		ErrorParameters error = msg.Data.Parameters.Deserialize<ErrorParameters>()!;
		
		_tcs.SetException((Exception)Activator.CreateInstance(ExceptionsRegistry.Exceptions[error.Message])!);
	}
}