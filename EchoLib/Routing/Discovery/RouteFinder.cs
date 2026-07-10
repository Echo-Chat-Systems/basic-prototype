using System.Linq.Expressions;
using System.Reflection;
using EchoLib.Core.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace EchoLib.Routing.Discovery;

public static class RouteFinder
{
	private static RouteTable? _cache = null;

	public static RouteTable Discover(IServiceProvider services)
	{
		if (_cache != null) return _cache;

		// Build new route table
		Dictionary<(string target, string action), RouteDescriptor> routes = new();

		List<Type> targets = Assembly.GetEntryAssembly()!
			.GetTypes()
			.Where(t => typeof(ITarget).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false }).ToList();

		foreach (Type target in targets)
		{
			ITarget targetInstance = (ITarget)services.GetRequiredService(target);

			IEnumerable<MethodInfo> actions = target.GetMethods()
				.Where(m => m.GetCustomAttribute<RouteAttribute>() != null);

			foreach (MethodInfo action in actions)
			{
				string actionName = action.GetCustomAttribute<RouteAttribute>()!.ActionName;

				ParameterInfo[] parameters = action.GetParameters();


				// Ensure correct number of parameters and correct types
				if (
					parameters.Length != 2 ||
					parameters[0].ParameterType != typeof(RouteContext)
				) throw new InvalidOperationException($"Method {action.Name}(routed as {actionName}) must be ({nameof(RouteContext)}, T)");


				routes.Add((targetInstance.Name, actionName), del);
			}
		}

		return new RouteTable { Routes = routes };
	}

	private static Func<object, RouteContext, object, Task<object?>> CompileHandler(ITarget instance, MethodInfo method)
	{
		ParameterExpression ctxParameter = Expression.Parameter(typeof(RouteContext));
		ParameterExpression requestParameter = Expression.Parameter(typeof(object));

		UnaryExpression requestCast = Expression.Convert(requestParameter, method.GetParameters()[1].ParameterType);
		var instanceExpression =
			Expression.Constant(instance);


		var call =
			Expression.Call(
				instanceExpression,
				method,
				ctxParameter,
				requestCast);


		var responseTask =
			Expression.Call(
				typeof(Route),
				nameof(ConvertResponse),
				null,
				call);


		return Expression
			.Lambda<Func<RouteContext, object, Task<object?>>>(
				responseTask,
				contextParameter,
				requestParameter)
			.Compile();
	}
}