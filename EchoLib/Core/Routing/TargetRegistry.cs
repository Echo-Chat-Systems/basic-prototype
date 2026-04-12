using System.Reflection;

namespace EchoLib.Core.Routing;

public static class TargetRegistry
{
	public static readonly Dictionary<string, ITarget> Targets = Discover();

	public static T? GetTarget<T>()
	{
		return (T?)Targets.Values.FirstOrDefault(t => t is T);
	}

	private static Dictionary<string, ITarget> Discover()
	{
		// This is complicated, so requires an explanation
		return Assembly.GetEntryAssembly()
			.GetTypes()
			.Where( // Get the types of all targets within the executing assembly 
				t => typeof(ITarget)
					     .IsAssignableFrom(t)
				     && t is { IsAbstract: false, IsInterface: false }
			)
			.ToList()
			.Select( // Create a new instance of every target and register it under it's name. 
				targetType => (ITarget)Activator.CreateInstance(targetType)!).ToDictionary(target => target.Name
			);
	}
}