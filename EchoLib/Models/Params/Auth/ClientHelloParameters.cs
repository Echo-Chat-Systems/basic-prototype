using EchoLib.Core.Crypto.Signing;
using EchoLib.Models.Crypto;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class ClientHelloParameters : IParam
{
	public string Action => "client-hello";

	[JsonProperty("key-pair")] public required PublicKeyPairJm KeyPair { get; init; }
}