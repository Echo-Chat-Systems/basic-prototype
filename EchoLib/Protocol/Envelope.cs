using System.Text.Json;

namespace EchoLib.Protocol;

public sealed class Envelope
{
	public Guid? MessageId { get; init; }
	public required string Target { get; init; }
	public required MessageData Data { get; init; }
}

public sealed class MessageData
{
	public required string Action { get; init; }
	public required JsonElement Parameters { get; init; }
}