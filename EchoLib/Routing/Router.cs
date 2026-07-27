using System.Text.Json;
using EchoLib.Core;
using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing.Discovery;
using EchoLib.Routing.Identification;
using EchoLib.Routing.Responses;
using EchoLib.Routing.Storage;
using EchoLib.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharper;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace EchoLib.Routing;

public sealed class Router
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<Router> _logger;

	private readonly RouteRegistry _routes;
	private readonly TargetInstanceRegistry _targets;
	private readonly PendingResponseRegistry _pendingResponses;

	public Router(IServiceProvider serviceProvider)
	{
		// Get registries from service provider
		_serviceProvider = serviceProvider;
		_logger = serviceProvider.GetRequiredService<ILogger<Router>>();
		_routes = new RouteRegistry();
		_targets = new TargetInstanceRegistry();
		_pendingResponses = serviceProvider.GetRequiredService<PendingResponseRegistry>();

		// Find routes (cached, so it's fine to run this in non-static code)
		RouteFinder.Discover(serviceProvider, _routes, _targets);

		serviceProvider.GetService<ITargetHub>()?
			.Populate(_targets);
	}
	
	public async void Receive(Envelope<JToken> message, WebSocket socket)
	{
		// Check if the envelope has an MID
		if (_pendingResponses.TryRemove(message.MessageId, out IPendingRequest? request))
		{
			if (message.Target == "error") request!.Fail(message);
			else request!.Complete(message);
			return;
		}
		
		RouteDescriptor? route = _routes.Get(message.Target, message.Data.Action);
		if (route == null)
			throw new InvalidOperationException($"Couldn't find route for {message.Target}:{message.Data.Action}");

		try
		{
			await route.Invoke(new RoutingContext
			{
				OriginalMessage = message,
				MessageId = message.MessageId,
				Socket = socket,
				Endpoint = new WebsocketEndpoint(socket, _serviceProvider)

			}, message.Data.Parameters);
		}
		catch (ProtocolException ex)
		{
			await new WebsocketEndpoint(socket, _serviceProvider).ErrorAsync(ex, message.MessageId);
		}
		catch (Exception ex)
		{
			InternalServerException iex = new();
			_logger.LogError("[{ErrorId}] [{ErrorName}] : \n{Stacktrace}", iex.Eid, ex.Message, ex.StackTrace);
			await new WebsocketEndpoint(socket, _serviceProvider).ErrorAsync(iex, message.MessageId);
		}
	}
}

public static class RoutingServiceCollectionExtensions
{
	public static IServiceCollection AddRouting(this IServiceCollection services)
	{
		// Configure Json Serializer
		JsonConvert.DefaultSettings = Json.DefaultSettings;

		// Register exceptions
		ExceptionsRegistry.Find();

		services.AddSingleton<Router>();
		services.AddSingleton<PendingResponseRegistry>();
		services.AddSingleton<JsonSerializer>(_ => JsonSerializer.Create(Json.DefaultSettings()));
		
		Type? targetHub = ImplementationFinder.Find<ITargetHub>();
		if (targetHub != null)
		{
			// Add as generic
			services.AddSingleton(typeof(ITargetHub), targetHub);

			// Add mapping to use generic implementation
			services.AddSingleton(targetHub, s => s.GetService<ITargetHub>()!);
		}

		return services;
	}
}