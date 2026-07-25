using Microsoft.Extensions.DependencyInjection;
using Server.Database.ParameterConverters;

namespace Server.Database.Discovery;

public static class DatabaseServicesExtensions
{
	public static IServiceCollection AddDatabase(this IServiceCollection services)
	{
		services.AddScoped<IDbConnectionProvider, PgDbConnectionProvider>();
		services.AddTransient<DbHub>();
		
		// Database info
		Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
		Dapper.SqlMapper.AddTypeHandler(new PublicSigningKeyConverter());
		Dapper.SqlMapper.AddTypeHandler(new PublicEncryptionKeyConverter());

		// Get all repos
		foreach (KeyValuePair<Type, Type> repo in RepoFinder.GetRepositories())
		{
			services.AddScoped(repo.Key, repo.Value);
		}

		return services;
	}
}

