using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace Models.Crypto.Signing;

public class Signature
{
	// Internal value store
	private readonly byte[] _value;

	/// <summary>
	/// B64 value of the signature.
	/// </summary>
	public string Value => Convert.ToBase64String(_value);

	/// <summary>
	/// Initialises from byte array.
	/// </summary>
	/// <param name="value">Signature bytes.</param>
	public Signature(byte[] value)
	{
		_value = value;
	}

	/// <summary>
	/// Initialises from B64 signature string.
	/// </summary>
	/// <param name="value">B64 String of signature</param>
	public Signature(string value)
	{
		_value = Convert.FromBase64String(value);
	}

	/// <summary>
	/// Checks if the
	/// </summary>
	/// <param name="pubKey">Public key</param>
	/// <param name="expected">Expected value</param>
	/// <returns>If the signature is valid.</returns>
	public bool Verify(Ed25519PublicKeyParameters pubKey, byte[] expected)
	{
		Ed25519Signer verifier = new();
		verifier.Init(false, pubKey);
		verifier.BlockUpdate(expected, 0, expected.Length);

		return verifier.VerifySignature(_value);
	}

	public override string ToString()
	{
		return Convert.ToBase64String(_value);
	}
}