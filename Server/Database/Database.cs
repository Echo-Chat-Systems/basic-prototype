using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Database;

public class Database
{
	public static IServiceProvider Repositories { get; private set; }
	public static DatabaseProvider CurrentProvider { get; private set; }
	public static IReadOnlyDictionary<Type, Func<object>> Implementations { get; private set; }

	public static void Initialise(DatabaseProvider provider, DatabaseContext ctx, IDatabaseProviderModule module)
	{
		if (module.Provider != provider) throw new InvalidOperationException("Provider mismatch");

		RepositoryRegistry registry = new();
		module.Register(ctx, registry);

		CurrentProvider = provider;
		Implementations = registry.Snapshot();

		ServiceCollection repositories = new();

		foreach (KeyValuePair<Type, Func<object>> implementation in Implementations) repositories.AddSingleton(implementation.Key, _ => implementation.Value());

		Repositories = repositories.BuildServiceProvider();
	}
}

public enum DatabaseProvider
{
	Postgres
}