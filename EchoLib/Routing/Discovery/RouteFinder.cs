using System.Reflection;
using EchoLib.Routing.Identification;
using EchoLib.Routing.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EchoLib.Routing.Discovery;

public static class RouteFinder
{
	private static Dictionary<(string target, string action), RouteDescriptor>? _cache = null;

	public static void Discover(IServiceProvider services, RouteRegistry routeRegistry, TargetInstanceRegistry targetRegistry)
	{
		if (_cache != null)
			foreach (KeyValuePair<(string target, string action), RouteDescriptor> kvp in _cache)
				routeRegistry.RegisterRoute(kvp.Value);

		;

		// Build new route table
		List<Type> targets = Assembly.GetEntryAssembly()!
			.GetTypes()
			.Where(t => typeof(ITarget).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
			.ToList();

		foreach (Type target in targets)
		{
			ITarget targetInstance = (ITarget)ActivatorUtilities.CreateInstance(services, target);
			targetRegistry.Register(target, targetInstance);

			IEnumerable<MethodInfo> actions = target.GetMethods()
				.Where(m => m.GetCustomAttribute<RouteAttribute>() != null);

			foreach (MethodInfo action in actions)
			{
				string actionName = action.GetCustomAttribute<RouteAttribute>()!.ActionName;

				ParameterInfo[] parameters = action.GetParameters();

				// Ensure correct number of parameters and correct types
				if (
					parameters.Length != 2 ||
					parameters[0].ParameterType != typeof(RoutingContext)
				) throw new InvalidOperationException($"Method {action.Name}(routed as {actionName}) must be ({nameof(RoutingContext)}, T)");


				routeRegistry.RegisterRoute(
					RouteDescriptorFactory.Create(
						action.GetCustomAttributes<BasePreProcessorAttribute>(),
						targetInstance, action, targetInstance.Name, actionName)
				);
			}
		}
	}
}