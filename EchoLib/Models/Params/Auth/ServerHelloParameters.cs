using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class ServerHelloParameters : IParam
{
	public string Action => "server-hello";

	[JsonProperty("server-name")] public required string ServerName { get; set; }
}