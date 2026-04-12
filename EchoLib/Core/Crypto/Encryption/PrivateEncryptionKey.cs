using System.Security.Cryptography;
using Newtonsoft.Json;

namespace EchoLib.Core.Crypto.Encryption;

public class PrivateEncryptionKey
{
	private RSA Key { get; }

	public PrivateEncryptionKey(byte[] key)
	{
		Key = RSA.Create();
		Key.ImportRSAPrivateKey(key, out _);
	}

	public PrivateEncryptionKey(string key)
	{
		Key = RSA.Create();
		Key.ImportRSAPrivateKey(Convert.FromBase64String(key), out _);
	}

	public bool Decrypt(byte[] data, out byte[] decryptedData)
	{
		try
		{
			decryptedData = Key.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
			return true;
		}
		catch
		{
			decryptedData = [];
			return false;
		}
	}

	public override string ToString()
	{
		return Convert.ToBase64String(Key.ExportRSAPrivateKey());
	}
}

public sealed class PrivateEncryptionKeyConverter
	: JsonConverter<PrivateEncryptionKey>
{
	public override void WriteJson(JsonWriter writer, PrivateEncryptionKey? value, JsonSerializer serializer)
	{
		writer.WriteValue(value?.ToString());
	}

	public override PrivateEncryptionKey ReadJson(
		JsonReader reader,
		Type objectType,
		PrivateEncryptionKey? existingValue,
		bool hasExistingValue,
		JsonSerializer serializer)
	{
		return reader.TokenType != JsonToken.String ? throw new JsonSerializationException("Expected string") : new PrivateEncryptionKey((string)reader.Value!);
	}
}