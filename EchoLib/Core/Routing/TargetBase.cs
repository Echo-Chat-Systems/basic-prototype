using System.Reflection;

namespace EchoLib.Core.Routing;

public abstract class TargetBase : ITarget
{
	public abstract string Name { get; }
	protected readonly RoutingContext _ctx;

	// Dictionary of action → method delegate (startup only reflection)
	private readonly Dictionary<string, Func<object, Task>> _actionHandlers = new();

	// Dictionary of action → list of callbacks
	private readonly Dictionary<string, List<Delegate>> _callbacks = new();

	protected TargetBase(RoutingContext ctx)
	{
		_ctx = ctx;
		InitializeActionHandlers();
	}

	private void InitializeActionHandlers()
	{
		IEnumerable<MethodInfo> methods = GetType()
			.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
			.Where(m => m.GetCustomAttribute<ActionHandlerAttribute>() != null);

		foreach (MethodInfo method in methods)
		{
			ActionHandlerAttribute attr = method.GetCustomAttribute<ActionHandlerAttribute>()!;
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length != 1)
				throw new InvalidOperationException($"Method {method.Name} must take exactly 1 parameter");

			Type paramType = parameters[0].ParameterType;

			// Wrap method as a Func<object, Task>
			Func<object, Task> del = param =>
			{
				object casted = Convert.ChangeType(param, paramType);
				object? result = method.Invoke(this, new[] { casted });
				return result is Task t ? t : Task.CompletedTask;
			};

			_actionHandlers[attr.ActionName] = del;
		}
	}

	public async Task HandleAsync(MessageEnvelope<object> envelope, RoutingContext ctx)
	{
		// Invoke callbacks first
		if (_callbacks.TryGetValue(envelope.Data.Action, out List<Delegate>? callbacks))
			foreach (Delegate cb in callbacks)
				await ((Func<object, Task>)cb)(envelope.Data.Params);

		// Invoke internal handler if exists
		if (_actionHandlers.TryGetValue(envelope.Data.Action, out Func<object, Task>? handler)) await handler(envelope.Data.Params);
	}

	/// <summary>
	/// Register a callback for a specific action.
	/// </summary>
	protected void RegisterCallback<TParams>(string action, Func<TParams, Task> callback)
	{
		if (!_callbacks.TryGetValue(action, out List<Delegate>? list))
		{
			list = new List<Delegate>();
			_callbacks[action] = list;
		}

		list.Add(new Func<object, Task>(p => callback((TParams)p)));
	}
}