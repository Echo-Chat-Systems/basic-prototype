using System.Text.Json;
using System.Text.Json.Serialization;
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

public class UserId : PublicSigningKey
{
	public UserId(string key) : base(key)
	{
	}

	public UserId(byte[] key) : base(key)
	{
	}
}

public sealed class PublicSigningKeyJsonConverter
	: JsonConverter<PublicSigningKey>
{
	public override PublicSigningKey Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options)
	{
		return reader.TokenType != JsonTokenType.String ? throw new JsonException() : new PublicSigningKey(reader.GetString()!);
	}

	public override void Write(
		Utf8JsonWriter writer,
		PublicSigningKey value,
		JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
}