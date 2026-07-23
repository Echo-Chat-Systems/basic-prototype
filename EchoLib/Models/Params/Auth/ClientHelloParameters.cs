using EchoLib.Models.Crypto;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class ClientHelloParameters : IParam
{
	public string Action => "hello";

	[JsonProperty("key-pair")] public required PublicKeyPairJm KeyPair { get; init; }
}