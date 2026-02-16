using System.Text.Json.Serialization;
using Models.Crypto.Encryption;
using Models.Functional.Crypto.Signing;

namespace EchoLib.Models.Params.Auth;

public class SigninStartParameters : IParam
{
	public string Action => "signin-start";
	
	[JsonPropertyName("sk")] [JsonConverter(typeof(PublicSigningKeyJsonConverter))]
	public PublicSigningKey sk { get; init; }
	
	[JsonPropertyName("ek")] [JsonConverter(typeof(PublicEncryptionKeyJsonConverter))]
	public PublicEncryptionKey ek { get; init; }
}