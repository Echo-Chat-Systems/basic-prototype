using EchoLib.Models.Crypto;
using EchoLib.Models.Misc;
using Newtonsoft.Json;

namespace EchoLib.Models;

public class JUserFile
{
	[JsonProperty("keys")] public required JKeySet Keys { get; set; }
	[JsonProperty("server")] public required ServerInfoJm Server { get; set; }
}