using EchoLib.Core.Crypto.Encryption;
using EchoLib.Core.Crypto.Signing;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class SigninStartParameters : IParam
{
	public string Action => "signin-start";

	[JsonProperty("sk")] public required PublicSigningKey Sk { get; init; }

	[JsonProperty("ek")] public required PublicEncryptionKey Ek { get; init; }
}