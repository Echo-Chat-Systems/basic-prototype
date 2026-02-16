using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

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

public sealed class PrivateEncryptionKeyJsonConverter
	: JsonConverter<PrivateEncryptionKey>
{
	public override PrivateEncryptionKey Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options)
	{
		return reader.TokenType != JsonTokenType.String ? throw new JsonException() : new PrivateEncryptionKey(reader.GetString()!);
	}

	public override void Write(
		Utf8JsonWriter writer,
		PrivateEncryptionKey value,
		JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
}