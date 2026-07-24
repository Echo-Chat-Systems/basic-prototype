using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;
using Newtonsoft.Json;

namespace EchoLib.Models.Crypto;

/// <summary>
/// All user keys.
/// </summary>
public class JKeySet
{
	[JsonProperty("pub_sk")] public PublicSigningKey PubSk { get; set; }

	[JsonProperty("prv_sk")] public PrivateSigningKey PrvSk { get; set; }

	[JsonProperty("pub_ek")] public PublicEncryptionKey PubEk { get; set; }

	[JsonProperty("prv_ek")] public PrivateEncryptionKey PrvEk { get; set; }

	public JPublicKeyPair ToPublicKeyPair()
	{
		return new JPublicKeyPair
		{
			SigningKey = PubSk,
			EncryptionKey = PubEk
		};
	}
}