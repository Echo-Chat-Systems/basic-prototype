using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Models.Crypto.Encryption;
using Models.Functional.Crypto.Signing;

namespace EchoLib.Models.Crypto;

public class PublicKeyPairJm
{
	[JsonPropertyName("sk")] [Required] public PublicSigningKey? SigningKey { get; set; }

	[JsonPropertyName("ek")] [Required] public PublicEncryptionKey? EncryptionKey { get; set; }
}