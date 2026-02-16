using System.Text.Json.Serialization;
using EchoLib.Core.Crypto.Encryption;
using Models.Crypto.Encryption;
using Models.Crypto.Signing;
using Models.Functional.Crypto.Signing;

namespace EchoLib.Models.Crypto;

/// <summary>
/// All user keys.
/// </summary>
public class KeySetJm
{
	[JsonPropertyName("pub_sk")]
	[JsonConverter(typeof(PublicSigningKeyJsonConverter))]
	public PublicSigningKey PubSk { get; set; }

	[JsonPropertyName("prv_sk")]
	[JsonConverter(typeof(PrivateSigningKeyJsonConverter))]
	public PrivateSigningKey PrvSk { get; set; }

	[JsonPropertyName("pub_ek")]
	[JsonConverter(typeof(PublicEncryptionKeyJsonConverter))]
	public PublicEncryptionKey PubEk { get; set; }

	[JsonPropertyName("prv_ek")]
	[JsonConverter(typeof(PrivateEncryptionKeyJsonConverter))]
	public PrivateEncryptionKey PrvEk { get; set; }
}