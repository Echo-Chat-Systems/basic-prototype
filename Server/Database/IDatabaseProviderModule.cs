namespace Server.Database;

public interface IDatabaseProviderModule
{
	DatabaseProvider Provider { get; }

	void Register(DatabaseContext ctx, RepositoryRegistry registry);
}