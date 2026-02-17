using System.Text.Json.Serialization;
using EchoLib.Core.Crypto.Signing;
using Models.Crypto.Encryption;

namespace EchoLib.Models.Params.Auth;

public class SigninStartParameters : IParam
{
	public string Action => "signin-start";
	
	[JsonPropertyName("sk")] [JsonConverter(typeof(PublicSigningKeyJsonConverter))]
	public required PublicSigningKey Sk { get; init; }
	
	[JsonPropertyName("ek")] [JsonConverter(typeof(PublicEncryptionKeyJsonConverter))]
	public required PublicEncryptionKey Ek { get; init; }
}