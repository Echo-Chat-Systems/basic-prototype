using System.Security.Cryptography;

namespace Models.Crypto.Encryption;

public class PublicEncryptionKey
{
	private RSA Key { get; }

	public PublicEncryptionKey(byte[] key)
	{
		Key = RSA.Create();
		Key.ImportRSAPublicKey(key, out _);
	}

	public PublicEncryptionKey(string key)
	{
		Key = RSA.Create();
		Key.ImportRSAPublicKey(Convert.FromBase64String(key), out _);
	}

	public byte[] Encrypt(byte[] data)
	{
		return Key.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
	}

	public override string ToString()
	{
		return Convert.ToBase64String(Key.ExportRSAPublicKey());
	}
}