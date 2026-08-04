using System.Text.Json;
using EchoLib.Models.Params.Generic;
using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing.Storage;
using Newtonsoft.Json.Linq;

namespace EchoLib.Routing.Responses;

public interface IPendingRequest
{
	Guid MessageId { get; }
	Type ResponseType { get; }

	void Complete(Envelope<JToken> msg);
	void Fail(Envelope<JToken> msg);
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

	public void Complete(Envelope<JToken> msg)
	{
		// Deserialise into T
		T response = msg.Data.Parameters.ToObject<T>() ?? throw new InvalidOperationException("Could not deserialise parameters");

		_tcs.SetResult(response);
	}

	public void Fail(Envelope<JToken> msg)
	{
		// Deserialise into error params
		ErrorParameters error = msg.Data.Parameters.ToObject<ErrorParameters>()!;

		_tcs.SetException((Exception)Activator.CreateInstance(ExceptionsRegistry.Exceptions[error.Message])!);
	}
}