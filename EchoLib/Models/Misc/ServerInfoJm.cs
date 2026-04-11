using System.Text.Json.Serialization;

namespace EchoLib.Models.Misc;

public class ServerInfoJm
{
	[JsonPropertyName("address")] public required string Address { get; set; }
	[JsonPropertyName("port")] public required int Port { get; set; }
	[JsonPropertyName("version")] public required string Version { get; set; }
}