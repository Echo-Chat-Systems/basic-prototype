using System.Security.Cryptography;
using Newtonsoft.Json;

namespace EchoLib.Crypto.Encryption;

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

public sealed class PublicEncryptionKeyJsonConverter
	: JsonConverter<PublicEncryptionKey>
{
	public override void WriteJson(JsonWriter writer, PublicEncryptionKey? value, JsonSerializer serializer)
	{
		writer.WriteValue(value?.ToString());
	}

	public override PublicEncryptionKey ReadJson(
		JsonReader reader,
		Type objectType,
		PublicEncryptionKey? existingValue,
		bool hasExistingValue,
		JsonSerializer serializer)
	{
		return reader.TokenType != JsonToken.String ? throw new JsonSerializationException("Expected string") : new PublicEncryptionKey((string)reader.Value!);
	}
}