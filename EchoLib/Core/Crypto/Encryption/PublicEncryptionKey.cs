using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Models.Crypto.Encryption;

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
	public override PublicEncryptionKey Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options)
	{
		return reader.TokenType != JsonTokenType.String ? throw new JsonException() : new PublicEncryptionKey(reader.GetString()!);
	}

	public override void Write(
		Utf8JsonWriter writer,
		PublicEncryptionKey value,
		JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
}
