using EchoLib.Protocol.Models.Crypto;
using Newtonsoft.Json;

namespace EchoLib.Protocol.Models.Params.Auth;

public class ClientHelloParameters : IParam
{
	public string Action => "client-hello";

	[JsonProperty("key-pair")] public required PublicKeyPairJm KeyPair { get; init; }
}