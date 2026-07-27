using EchoLib.Configuration.Attributes;

namespace Server;

public class Config
{
	[ConfigProperty] public SocketModel Socket { get; init; }
	[ConfigProperty] public AppearanceModel Appearance { get; init; }
	[ConfigProperty] public DatabaseModel Database { get; init; }

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
		public required string BroadcastName { get; init; }
	}

	[ConfigModel]
	public class DatabaseModel
	{
		public required string Host { get; init; }
		public required string Name { get; init; }
		public required int Port { get; init; }
		public required IReadOnlyDictionary<string, CredentialsSet> Credentials { get; init; }

		public string CreateConnectionString(string cred)
		{
			return $"Host={Host};" +
			       $"Port={Port};" +
			       $"Database={Name};" +
			       $"Username={Credentials[cred].Username};" +
			       $"Password={Credentials[cred].Password};"
#if DEBUG
			       + $"Include Error Detail=true;";
#endif
		}
	}

	[ConfigModel]
	public class CredentialsSet
	{
		public required string Username { get; init; }
		[ConfigSecret] public required string Password { get; init; }
	}
}