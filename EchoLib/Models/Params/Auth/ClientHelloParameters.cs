using EchoLib.Core.Crypto.Signing;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class ClientHelloParameters : IParam
{
	public string Action => "client-hello";

	[JsonProperty("id")] public required PublicSigningKey Id { get; init; }
}