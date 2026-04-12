using EchoLib.Core.Crypto.Encryption;
using EchoLib.Core.Crypto.Signing;
using Newtonsoft.Json;

namespace EchoLib.Core;

public static class NewtonsoftJson
{
	public static JsonSerializerSettings DefaultSettings()
	{
		return new JsonSerializerSettings
		{
			Converters =
			[
				new PublicSigningKeyConverter(),
				new PrivateSigningKeyConverter(),
				new PublicEncryptionKeyJsonConverter(),
				new PrivateEncryptionKeyConverter()
			]
		};
	}
}