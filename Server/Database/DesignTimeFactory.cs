using EchoLib.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Server.Database;

public class DesignTimeFactory : IDesignTimeDbContextFactory<EchoContext> {

	public EchoContext CreateDbContext(string[] args)
	{
		// Read in config and .env files
		IConfiguration iConfiguration = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddJsonFile("appsettings.json")
			.AddIniFile("secrets.ini")
			.Build();

		// Use config library to build config class
		Config config = ConfigBuilder.Build<Config>(iConfiguration);

		DbContextOptionsBuilder opt = new DbContextOptionsBuilder<EchoContext>()
			.UseNpgsql(config.Database.CreateConnectionString("Main"));

		EchoContext.ConnectionString = config.Database.CreateConnectionString("Main");

		return new EchoContext(opt.Options);
	}
}