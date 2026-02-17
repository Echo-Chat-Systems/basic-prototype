using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EchoLib.Core.Crypto.Signing;
using Models.Crypto.Encryption;

namespace EchoLib.Models.Crypto;

public class PublicKeyPairJm
{
	[JsonPropertyName("sk")] [Required] public PublicSigningKey? SigningKey { get; set; }

	[JsonPropertyName("ek")] [Required] public PublicEncryptionKey? EncryptionKey { get; set; }
}