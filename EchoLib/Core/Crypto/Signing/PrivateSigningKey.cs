using System.Buffers.Text;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Org.BouncyCastle.Crypto.Parameters;
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
		KeyParams.Sign(Ed25519.Algorithm.Ed25519, null, sign, output);

		return output.ToArray();
	}

	public override string ToString()
	{
		return Convert.ToBase64String(Key);
	}
}

public sealed class PrivateSigningKeyJsonConverter
	: JsonConverter<PrivateSigningKey>
{
	public override PrivateSigningKey Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options)
	{
		return reader.TokenType != JsonTokenType.String ? throw new JsonException() : new PrivateSigningKey(reader.GetString()!);
	}

	public override void Write(
		Utf8JsonWriter writer,
		PrivateSigningKey value,
		JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
}