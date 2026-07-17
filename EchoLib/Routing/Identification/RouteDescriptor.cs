using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace EchoLib.Routing.Identification;

public abstract class RouteDescriptor
{
	public abstract string Target { get; init; }

	public abstract string Action { get; init; }

	public abstract Type RequestType { get; init; }


	public abstract Task Invoke(RoutingContext ctx, JToken json);
}

public sealed class RouteDescriptor<TRequest>(Func<RoutingContext, TRequest, Task> handler) : RouteDescriptor
{
	public override required string Target { get; init; }
	public override required string Action { get; init; }
	public override Type RequestType { get; init; } =  typeof(TRequest);

	public override Task Invoke(RoutingContext ctx, JToken json)
	{
		TRequest request = json.ToObject<TRequest>() ?? throw new InvalidOperationException("Unable to deserialize request");
		
		 return handler(ctx, request);
	}
}

public static class RouteDescriptorFactory
{
	public static RouteDescriptor Create(
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
					instance,
					method,
					target,
					action
				])!;
	}

	private static RouteDescriptor CreateGeneric<TRequest>(
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
			handler)
		{
			Target = target,
			Action = action
		};
	}
}