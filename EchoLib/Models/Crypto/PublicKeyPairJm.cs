using System.ComponentModel.DataAnnotations;
using EchoLib.Core.Crypto.Encryption;
using EchoLib.Core.Crypto.Signing;
using Newtonsoft.Json;

namespace EchoLib.Models.Crypto;

public class PublicKeyPairJm
{
	[JsonProperty("sk")] [Required] public PublicSigningKey? SigningKey { get; set; }

	[JsonProperty("ek")] [Required] public PublicEncryptionKey? EncryptionKey { get; set; }
}