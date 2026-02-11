using Server.Database.Repositories;

namespace Server.Database;

public sealed class RepositoryRegistry
{
	private readonly Dictionary<Type, Func<object>> _factories = new();

	public void Register<TInterface>(Func<TInterface> factory)
		where TInterface : class, IRepo
	{
		_factories[typeof(TInterface)] = factory;
	}

	public bool IsImplemented<TInterface>()
		where TInterface : class, IRepo
	{
		return _factories.ContainsKey(typeof(TInterface));
	}

	public TInterface Resolve<TInterface>()
		where TInterface : class, IRepo
	{
		if (!_factories.TryGetValue(typeof(TInterface), out Func<object> factory)) throw new InvalidOperationException($"{typeof(TInterface)} not implemented");

		return (TInterface)factory();
	}

	public IReadOnlyDictionary<Type, Func<object>> Snapshot() => _factories;
}