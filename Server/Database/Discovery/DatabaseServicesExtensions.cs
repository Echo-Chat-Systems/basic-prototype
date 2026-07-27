using Dapper;
using EchoLib.Models.Data;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Data.Guild;
using EchoLib.Models.Data.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Database.ParameterConverters;

namespace Server.Database.Discovery;

public static class DatabaseServicesExtensions
{
	public static IServiceCollection AddDatabase(this IServiceCollection services)
	{
		services.AddScoped<IDbConnectionProvider, PgDbConnectionProvider>();
		services.AddTransient<DbHub>();
		
		// Database info
		DefaultTypeMap.MatchNamesWithUnderscores = true;
		SqlMapper.AddTypeHandler(new PublicSigningKeyConverter());
		SqlMapper.AddTypeHandler(new PublicEncryptionKeyConverter());
		SqlMapper.AddTypeHandler(new SnowflakeConverter());

		// All DB-Stored JModels
		AddJsonbHandler<JProfile>();

		// Guilds
		AddJsonbHandler<JGuildConfig>();
		AddJsonbHandler<JGuildCustomisation>();

		// Roles
		AddJsonbHandler<JRoleCustomisation>();
		AddJsonbHandler<JRolePermissionSet>();

		// Channels
		AddJsonbHandler<JChannelCustomisation>();
		AddJsonbHandler<JChannelConfig>();

		// Get all repos
		foreach (KeyValuePair<Type, Type> repo in RepoFinder.GetRepositories())
		{
			services.AddScoped(repo.Key, repo.Value);
		}

		return services;
	}

	private static void AddJsonbHandler<T>() where T : class
	{
		SqlMapper.AddTypeHandler(new JsonbConverter<T>());
	}
}

public static class LoggerExtensions
{
	public static void LogDb(this ILogger logger, string methodName, object? para)
	{
		logger.LogDebug("[{Method}]: {Para}", methodName, para.ToString());
	}
}

