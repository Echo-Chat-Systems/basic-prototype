using System.Reflection;

namespace Server.Database.Discovery;

public static class RepoFinder
{
	public static Dictionary<Type, Type> GetRepositories()
	{
		Dictionary<Type, Type> repos = Assembly.GetExecutingAssembly()
			.GetTypes()
			.Where(t => t is { IsInterface: true } && t.GetCustomAttribute<RepoAttribute>() != null)
			.Select(t => new KeyValuePair<Type,Type>(t, t.GetCustomAttribute<RepoAttribute>()!.DefaultImplementation))
			.ToDictionary();

		// Ensure that every implementor is valid
		return repos.Any(repo => !repo.Value.IsAssignableTo(repo.Key)) ? throw new InvalidCastException() : repos;
	}
}