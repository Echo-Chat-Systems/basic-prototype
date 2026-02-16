namespace EchoLib.Core.Routing;

public sealed class Router : IMessageRouter
{
	private readonly Dictionary<Type, ITarget> _targets = new();
	private readonly RoutingContext _ctx;

	public Router(RoutingContext ctx)
	{
		_ctx = ctx;

		// Automatically discover and register all targets
		foreach (ITarget target in TargetDiscovery.DiscoverTargets(ctx)) _targets[target.GetType()] = target;
	}

	public TTarget Get<TTarget>() where TTarget : ITarget
	{
		if (!_targets.TryGetValue(typeof(TTarget), out ITarget? target))
			throw new InvalidOperationException($"Target {typeof(TTarget).Name} not registered");

		return (TTarget)target;
	}

	public Task RouteAsync(MessageEnvelope<object> envelope)
	{
		// Lookup by target string
		ITarget target = _targets.Values.FirstOrDefault(t => t.Name == envelope.Target)
		                 ?? throw new InvalidOperationException($"Unknown target: {envelope.Target}");

		return target.HandleAsync(envelope, _ctx);
	}
}