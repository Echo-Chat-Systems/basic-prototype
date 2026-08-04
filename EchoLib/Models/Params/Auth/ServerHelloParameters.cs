using EchoLib.Core;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class ServerHelloParameters : IParam
{
	public string Action => RouteNames.Auth.ServerHello;

	[JsonProperty("server-name")] public required string ServerName { get; set; }
}