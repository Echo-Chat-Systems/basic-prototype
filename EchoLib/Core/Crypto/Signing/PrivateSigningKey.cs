using System.Text.Json;
using System.Text.Json.Serialization;
using Models.Functional.Crypto.Signing;
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