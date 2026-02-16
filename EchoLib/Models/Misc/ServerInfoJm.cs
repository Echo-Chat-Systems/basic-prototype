using System.Text.Json.Serialization;

namespace EchoLib.Models.Misc;

public class ServerInfoJm
{
	[JsonPropertyName("address")] public required string Address;
	[JsonPropertyName("port")] public required int Port;
	[JsonPropertyName("version")] public required string Version;
}