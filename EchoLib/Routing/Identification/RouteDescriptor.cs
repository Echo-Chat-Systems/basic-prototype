using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace EchoLib.Routing.Identification;

public abstract class RouteDescriptor
{
	public abstract string Target { get; init; }

	public abstract string Action { get; init; }

	public abstract Type RequestType { get; init; }

	public abstract IEnumerable<BasePreProcessorAttribute> Preprocessors { get; init; }

	public abstract Task Invoke(RoutingContext ctx, JToken json);
}

public sealed class RouteDescriptor<TRequest>(Func<RoutingContext, TRequest, Task> handler, IEnumerable<BasePreProcessorAttribute> preprocessors) : RouteDescriptor
{
	public override required string Target { get; init; }
	public override required string Action { get; init; }
	public override Type RequestType { get; init; } = typeof(TRequest);

	public override IEnumerable<BasePreProcessorAttribute> Preprocessors { get; init; } = preprocessors;

	public override async Task Invoke(RoutingContext ctx, JToken json)
	{
		// Run preprocessors first
		foreach (BasePreProcessorAttribute task in Preprocessors)
			if (!await task.Run(ctx))
				return;

		TRequest request = json.ToObject<TRequest>() ?? throw new InvalidOperationException("Unable to deserialize request");

		await handler(ctx, request);
	}
}

public static class RouteDescriptorFactory
{
	public static RouteDescriptor Create(
		IEnumerable<BasePreProcessorAttribute> preprocessors,
		object instance,
		MethodInfo method,
		string target,
		string action)
	{
		ParameterInfo[] parameters =
			method.GetParameters();

		Type requestType =
			parameters[1].ParameterType;

		MethodInfo genericFactory =
			typeof(RouteDescriptorFactory)
				.GetMethod(
					nameof(CreateGeneric),
					BindingFlags.NonPublic | BindingFlags.Static)!
				.MakeGenericMethod(
					requestType);

		return (RouteDescriptor)
			genericFactory.Invoke(
				null,
				[
					preprocessors,
					instance,
					method,
					target,
					action
				])!;
	}

	private static RouteDescriptor CreateGeneric<TRequest>(
		IEnumerable<BasePreProcessorAttribute> preprocessors,
		object instance,
		MethodInfo method,
		string target,
		string action)
	{
		Func<RoutingContext, TRequest, Task> handler =
			(Func<RoutingContext, TRequest, Task>)
			Delegate.CreateDelegate(
				typeof(Func<RoutingContext, TRequest, Task>),
				instance,
				method);

		return new RouteDescriptor<TRequest>(
			handler,
			preprocessors
		)
		{
			Target = target,
			Action = action
		};
	}
}