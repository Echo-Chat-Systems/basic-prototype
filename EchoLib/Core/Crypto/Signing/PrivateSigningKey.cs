using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math.EC.Rfc8032;

namespace EchoLib.Core.Crypto.Signing;

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

	/// <summary>
	/// Sign any arbitrary byte array.
	/// </summary>
	/// <param name="sign">String to sign.</param>
	/// <returns>B64 encoded signature.</returns>
	public byte[] Sign(byte[] sign)
	{
		// Convert input string into a readonly span
		Span<byte> output = new();

		// Perform actual signature
		Ed25519Signer signer = new();
		signer.Init(true, KeyParams);
		signer.BlockUpdate(sign, 0, sign.Length);

		output = signer.GenerateSignature();

		// KeyParams.Sign(Ed25519.Algorithm.Ed25519, null, sign, output);

		return output.ToArray();
	}

	public override string ToString()
	{
		return Convert.ToBase64String(Key);
	}
}

public class PrivateSigningKeyConverter : JsonConverter<PrivateSigningKey>
{
	public override void WriteJson(JsonWriter writer, PrivateSigningKey? value, JsonSerializer serializer)
	{
		writer.WriteValue(value?.ToString());
	}

	public override PrivateSigningKey? ReadJson(JsonReader reader, Type objectType, PrivateSigningKey? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		return reader.TokenType != JsonToken.String ? throw new JsonSerializationException("Expected string") : new PrivateSigningKey((string)reader.Value!);
	}
}