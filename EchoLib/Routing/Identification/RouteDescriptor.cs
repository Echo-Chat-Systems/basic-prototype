using System.Reflection;
using System.Text.Json;

namespace EchoLib.Routing.Identification;

public abstract class RouteDescriptor
{
	public abstract string Target { get; init; }

	public abstract string Action { get; init; }

	public abstract Type RequestType { get; init; }

	public abstract Type ResponseType { get; init; }

	public abstract Task<object?> Invoke(RoutingContext ctx, JsonElement json);
}

public sealed class RouteDescriptor<TRequest, TResponse>(Func<RoutingContext, TRequest, Task<TResponse?>> handler) : RouteDescriptor
{
	public override required string Target { get; init; }
	public override required string Action { get; init; }
	public override Type RequestType { get; init; } =  typeof(TRequest);
	public override Type ResponseType { get; init; } =  typeof(TResponse);
	public override async Task<object?> Invoke(RoutingContext ctx, JsonElement json)
	{
		TRequest request = json.Deserialize<TRequest>() ?? throw new InvalidOperationException("Unable to deserialize request");
		
		TResponse? response = await handler(ctx, request);

		return response;
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

		Type responseType =
			method.ReturnType.GenericTypeArguments[0];

		MethodInfo genericFactory =
			typeof(RouteDescriptorFactory)
				.GetMethod(
					nameof(CreateGeneric),
					BindingFlags.NonPublic | BindingFlags.Static)!
				.MakeGenericMethod(
					requestType,
					responseType);

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

	private static RouteDescriptor CreateGeneric<TRequest, TResponse>(
		object instance,
		MethodInfo method,
		string target,
		string action)
	{
		Func<RoutingContext, TRequest, Task<TResponse>> handler =
			(Func<RoutingContext, TRequest, Task<TResponse>>)
			Delegate.CreateDelegate(
				typeof(Func<RoutingContext, TRequest, Task<TResponse>>),
				instance,
				method);

		return new RouteDescriptor<TRequest, TResponse>(
			handler)
		{
			Target = target,
			Action = action
		};
	}
}