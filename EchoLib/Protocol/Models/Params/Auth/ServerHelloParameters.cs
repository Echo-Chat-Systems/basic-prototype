using Newtonsoft.Json;

namespace EchoLib.Protocol.Models.Params.Auth;

public class ServerHelloParameters : IParam
{
	public string Action => "server-hello";

	[JsonProperty("server-name")] public required string ServerName { get; set; }
}