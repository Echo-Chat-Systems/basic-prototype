using Microsoft.Extensions.Configuration;

namespace Server.Config;

public class Config(IConfiguration config)
{
	public class DatabaseModel
	{
		public required string Host { get; init; }
		public required string Name { get; init; }
		public required string Password { get; init; }
		public required int Port { get; init; }
		public required string Username { get; init; }
	}

	public DatabaseModel Database { get; } = new()
	{
		Host = config["Database:Host"] ?? throw new MissingFieldException("Database:Host"),
		Port = int.Parse(config["Database:Port"] ?? throw new MissingFieldException("Database:Port")),
		Username = config["Database:Username"] ?? throw new MissingFieldException("Database:Username"),
		Name = config["Database:Name"] ?? throw new MissingFieldException("Database:Name"),
		Password = config["Database:Password"] ?? throw new MissingFieldException("Database:Password")
	};
}