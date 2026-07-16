using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace EchoLib.Protocol;


public sealed record Envelope<TParams>
{
	public Guid? MessageId { get; init; }
	public required string Target { get; init; }
	public required ActionWrapper<TParams> Data { get; init; }
}

public sealed record ActionWrapper<TParams>
{
	[JsonProperty("action")]
	public required string Action { get; init; }
	[JsonProperty("params")]
	public required TParams Parameters { get; init; }
}