using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;
using Newtonsoft.Json;

namespace EchoLib.Core;

public static class Json
{
	public static JsonSerializerSettings DefaultSettings()
	{
		return new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			Converters =
			[
				new PublicSigningKeyConverter(),
				new PrivateSigningKeyConverter(),
				new PublicEncryptionKeyJsonConverter(),
				new PrivateEncryptionKeyConverter(),
				new SnowflakeJsonConverter()
			]
		};
	}

	public static JsonSerializer Serializer => JsonSerializer.Create();
}