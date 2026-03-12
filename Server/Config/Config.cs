using EchoLib.Configuration.Attributes;
using Microsoft.Extensions.Configuration;

namespace Server.Config;

public class Config
{
	[ConfigProperty] public required SocketModel Socket { get; init; }
	[ConfigProperty] public required AppearanceModel Appearance { get; init; }
	[ConfigProperty] public required DatabaseModel Database { get; init; }

	[ConfigModel]
	public class SocketModel
	{
		public required string Host { get; init; }
		public required int Port { get; init; }
		public required bool UsingWss { get; init; }
	}

	[ConfigModel]
	public class AppearanceModel
	{
		public required string Name { get; init; }
	}

	[ConfigModel]
	public class DatabaseModel
	{
		public required string Host { get; init; }
		public required string Name { get; init; }
		public required int Port { get; init; }
		public required IReadOnlyDictionary<string, CredentialsSet> Credentials { get; init; }
	}

	[ConfigModel]
	public class CredentialsSet
	{
		public required string Username { get; init; }
		[ConfigSecret] public required string Password { get; init; }
	}
}