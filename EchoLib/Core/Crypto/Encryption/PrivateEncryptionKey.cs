using System.Security.Cryptography;

namespace Models.Crypto.Encryption;

public class PrivateEncryptionKey
{
	private RSA Key { get; }

	public PrivateEncryptionKey(byte[] key)
	{
		Key = RSA.Create();
		Key.ImportRSAPrivateKey(key, out _);
	}

	public PrivateEncryptionKey(string key)
	{
		Key = RSA.Create();
		Key.ImportRSAPrivateKey(Convert.FromBase64String(key), out _);
	}

	public bool Decrypt(byte[] data, out byte[] decryptedData)
	{
		try
		{
			decryptedData = Key.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
			return true;
		}
		catch
		{
			decryptedData = [];
			return false;
		}
	}

	public override string ToString()
	{
		return Convert.ToBase64String(Key.ExportRSAPrivateKey());
	}
}