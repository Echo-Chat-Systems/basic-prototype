using EchoLib.Core;
using EchoLib.Models.Crypto;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class ClientHelloParameters : IParam
{
	public string Action => RouteNames.Auth.ClientHello;

	[JsonProperty("key-pair")] public required JPublicKeyPair KeyPair { get; init; }
}