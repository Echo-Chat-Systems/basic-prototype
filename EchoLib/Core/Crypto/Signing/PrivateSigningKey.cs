using Org.BouncyCastle.Crypto.Parameters;

namespace Models.Crypto.Signing;

public class PrivateSigningKey
{
	private byte[] Key { get; }

	public Ed25519PrivateKeyParameters KeyParams => new(Key);

	public PrivateSigningKey(byte[] key)
	{
		Key = key;
	}

	public PrivateSigningKey(string key)
	{
		Key = Convert.FromBase64String(key);
	}

	public override string ToString()
	{
		return Convert.ToBase64String(Key);
	}
}