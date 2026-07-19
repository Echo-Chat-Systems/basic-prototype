using EchoLib.Models.Crypto;
using EchoLib.Models.Misc;
using Newtonsoft.Json;

namespace EchoLib.Models;

public class UserFileJm
{
	[JsonProperty("keys")] public required KeySetJm Keys { get; set; }
	[JsonProperty("server")] public required ServerInfoJm Server { get; set; }
}