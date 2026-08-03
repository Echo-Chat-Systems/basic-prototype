using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;

namespace EchoLib.Crypto.Signing;

public class PublicSigningKey : IEquatable<PublicSigningKey>
{
	public byte[] Key { get; }

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
	
	public static bool operator ==(PublicSigningKey s1, PublicSigningKey s2)
	{
		return s1.Equals(s2);
	}

	public static bool operator !=(PublicSigningKey s1, PublicSigningKey s2)
	{
		return !s1.Equals(s2);
	}

	public bool Equals(PublicSigningKey? other)
	{
		if (other is null) return false;
		if (ReferenceEquals(this, other)) return true;
		return Key.SequenceEqual(other.Key);
	}

	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != GetType()) return false;
		return Equals((PublicSigningKey)obj);
	}

	public override int GetHashCode()
	{
		return Key.GetHashCode();
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