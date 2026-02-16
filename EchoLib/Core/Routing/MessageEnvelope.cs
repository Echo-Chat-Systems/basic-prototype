using System.Text.Json.Serialization;

namespace EchoLib.Core.Routing;

public class MessageEnvelope<TParams>
{
	[JsonPropertyName("target")] public string Target { get; set; } = string.Empty;
	[JsonPropertyName("data")] public ActionWrapper<TParams> Data { get; set; } = new();
}