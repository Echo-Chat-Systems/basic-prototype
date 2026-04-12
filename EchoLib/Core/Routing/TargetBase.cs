using System.Reflection;
using EchoLib.Models.Params;
using Newtonsoft.Json.Linq;

namespace EchoLib.Core.Routing;

public abstract class TargetBase : ITarget
{
	public abstract string Name { get; }

	// Dictionary of action → method delegate (startup only reflection)
	private Dictionary<string, Func<RoutingContext, object, Task>> _actionHandlers = new();
	private static readonly Dictionary<Type, Dictionary<string, Func<RoutingContext, object, Task>>> Cache = new();

	protected TargetBase()
	{
		InitializeActionHandlers();
	}

	private void InitializeActionHandlers()
	{
		Type type = GetType();

		if (Cache.TryGetValue(type, out Dictionary<string, Func<RoutingContext, object, Task>>? cached))
		{
			_actionHandlers = cached;
			return;
		}

		Dictionary<string, Func<RoutingContext, object, Task>> built = new();

		IEnumerable<MethodInfo> methods = type
			.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
			.Where(m => m.GetCustomAttribute<ActionHandlerAttribute>() != null);

		foreach (MethodInfo method in methods)
		{
			ActionHandlerAttribute attr = method.GetCustomAttribute<ActionHandlerAttribute>()!;
			ParameterInfo[] parameters = method.GetParameters();

			Func<RoutingContext, object, Task> del;

			switch (parameters.Length)
			{
				case 1:
				{
					// (TParams)
					Type paramType = parameters[0].ParameterType;

					del = (ctx, rawParam) =>
					{
						// Ensure param is JObject
						JObject param = rawParam as JObject ?? throw new InvalidOperationException("Unable to deserialize params.");

						// Convert param to correct type
						object? casted = param.ToObject(paramType);

						object? result = method.Invoke(this, [casted]);
						return result as Task ?? Task.CompletedTask;
					};
					break;
				}
				case 2 when
					parameters[0].ParameterType == typeof(RoutingContext):
				{
					// (RoutingContext, TParams)
					Type paramType = parameters[1].ParameterType;

					del = (ctx, rawParam) =>
					{
						// Ensure param is JObject
						JObject param = rawParam as JObject ?? throw new InvalidOperationException("Unable to deserialize params.");

						// Convert param to correct type
						object? casted = param.ToObject(paramType);

						object? result = method.Invoke(this, [ctx, casted]);
						return result as Task ?? Task.CompletedTask;
					};
					break;
				}
				default:
					throw new InvalidOperationException(
						$"Method {method.Name} must be (T) or (RoutingContext, T)"
					);
			}

			built[attr.ActionName] = del;
		}

		Cache[type] = built;
		_actionHandlers = built;
	}

	public async Task HandleAsync(RoutingContext ctx, MessageEnvelope<object> envelope)
	{
		if (_actionHandlers.TryGetValue(envelope.Data.Action, out Func<RoutingContext, object, Task>? handler)) await handler(ctx, envelope.Data.Params);
	}
}