using Org.BouncyCastle.Crypto.Parameters;

namespace Models.Functional.Crypto.Signing;

public class PublicSigningKey
{
	private byte[] Key { get; }

	public Ed25519PublicKeyParameters KeyParams => new(Key);

	public PublicSigningKey(string key)
	{
		Key = Convert.FromBase64String(key);
	}

	public PublicSigningKey(byte[] key)
	{
		Key = key;
	}

	public override string ToString()
	{
		return Convert.ToBase64String(Key);
	}
}

public class UserId: PublicSigningKey
{
	public UserId(string key) : base(key)
	{
	}

	public UserId(byte[] key) : base(key)
	{
	}
}