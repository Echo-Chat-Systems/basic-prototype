using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;

namespace EchoLib.Crypto.Signing;

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

public class PublicSigningKeyConverter : JsonConverter<PublicSigningKey>
{
	public override void WriteJson(JsonWriter writer, PublicSigningKey? value, JsonSerializer serializer)
	{
		writer.WriteValue(value?.ToString());
	}

	public override PublicSigningKey ReadJson(
		JsonReader reader,
		Type objectType,
		PublicSigningKey? existingValue,
		bool hasExistingValue,
		JsonSerializer serializer)
	{
		return reader.TokenType != JsonToken.String ? throw new JsonSerializationException("Expected string") : new PublicSigningKey((string)reader.Value!);
	}
}