using Microsoft.Extensions.DependencyInjection;

namespace Server.Database.Discovery;

public static class DatabaseServicesExtensions
{
	public static IServiceCollection AddDatabase(this IServiceCollection services)
	{
		services.AddScoped<IDbConnectionProvider, PgDbConnectionProvider>();
		services.AddTransient<DbHub>();

		// Get all repos
		foreach (KeyValuePair<Type, Type> repo in RepoFinder.GetRepositories())
		{
			services.AddScoped(repo.Key, repo.Value);
		}

		return services;
	}
}

