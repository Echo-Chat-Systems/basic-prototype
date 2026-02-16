using System.Reflection;

namespace EchoLib.Core.Routing;

/// <summary>
/// Target discovery helper.
/// </summary>
public static class TargetDiscovery
{
	/// <summary>
	/// Find all classes implementing ITarget with a constructor taking RoutingContext, build an instance of them, and pass them to the list.
	/// </summary>
	/// <param name="ctx"></param>
	/// <returns></returns>
	public static IEnumerable<ITarget> DiscoverTargets(RoutingContext ctx)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();

		IEnumerable<Type> targetTypes = assembly
			.GetTypes()
			.Where(t => typeof(ITarget).IsAssignableFrom(t) // Translates to implements ITarget
			            && t is { IsAbstract: false, IsInterface: false } // Ensure we aren't grabbing interfaces or abstract classes 
			);

		foreach (Type type in targetTypes)
		{
			// Attempt to create an instance using RoutingContext constructor
			ConstructorInfo? constructor = type.GetConstructor(new[] { typeof(RoutingContext) });
			if (constructor != null) yield return (ITarget)constructor.Invoke([ctx]);
			else throw new InvalidOperationException($"Target {type.Name} must have a constructor accepting {nameof(RoutingContext)}");
		}
	}
}