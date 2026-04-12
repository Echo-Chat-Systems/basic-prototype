using Newtonsoft.Json;

namespace EchoLib.Models.Misc;

public class ServerInfoJm
{
	[JsonProperty("address")] public required string Address { get; set; }
	[JsonProperty("port")] public required int Port { get; set; }
	[JsonProperty("version")] public required string Version { get; set; }
}