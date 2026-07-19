using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class SigninResponseParameters : IParam
{
	public string Action => "signin-response";

	[JsonProperty("signature")] public required string Signature { get; init; }
	[JsonProperty("decrypted")] public required string Decrypted { get; init; }
}