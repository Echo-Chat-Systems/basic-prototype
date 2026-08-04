using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace EchoLib.Protocol;

public sealed record Envelope<TParams>
{
	[JsonPropertyName("mid")] public Guid MessageId { get; init; }

	[JsonPropertyName("target")] public required string Target { get; init; }

	[JsonPropertyName("data")] public required ActionWrapper<TParams> Data { get; init; }
}

public sealed record ActionWrapper<TParams>
{
	[JsonPropertyName("action")] public required string Action { get; init; }

	[JsonPropertyName("params")] public required TParams Parameters { get; init; }
}