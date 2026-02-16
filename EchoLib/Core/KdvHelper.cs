using System.Security.Cryptography;
using EchoLib.Core.Crypto.Encryption;
using EchoLib.Models.Crypto;
using Models.Crypto.Encryption;
using Models.Crypto.Signing;
using Models.Functional.Crypto.Signing;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace EchoLib.Core;

/// <summary>
/// Key Derivation Helper.
/// </summary>
public static class KdvHelper
{
	public static KeySetJm Generate()
	{
		// Generate encryption keys
		using RSA encryption = RSA.Create(2048);
		byte[] pubEk = encryption.ExportRSAPublicKey();
		byte[] prvEk = encryption.ExportRSAPrivateKey();

		// Generate signing keys (Ed25519)
		Ed25519KeyPairGenerator signing = new();
		signing.Init(new Ed25519KeyGenerationParameters(new SecureRandom()));

		AsymmetricCipherKeyPair keyPair = signing.GenerateKeyPair();
		byte[] pubSk = ((Ed25519PublicKeyParameters)keyPair.Public).GetEncoded();
		byte[] prvSk = ((Ed25519PrivateKeyParameters)keyPair.Private).GetEncoded();

		return new KeySetJm
		{
			PubSk = new PublicSigningKey(pubSk),
			PrvSk = new PrivateSigningKey(prvSk),
			PubEk = new PublicEncryptionKey(pubEk),
			PrvEk = new PrivateEncryptionKey(prvEk)
		};
	}
}