using System.ComponentModel.DataAnnotations;
using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;
using Newtonsoft.Json;

namespace EchoLib.Protocol.Models.Crypto;

public class PublicKeyPairJm
{
	[JsonProperty("sk")] [Required] public PublicSigningKey? SigningKey { get; set; }

	[JsonProperty("ek")] [Required] public PublicEncryptionKey? EncryptionKey { get; set; }
}