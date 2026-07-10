using Newtonsoft.Json;

namespace EchoLib.Protocol.Models.Data;

public class JErrorSourceData
{
	[JsonProperty("target")] public required string Target { get; init; }
	[JsonProperty("action")] public required string Action { get; init; }
}