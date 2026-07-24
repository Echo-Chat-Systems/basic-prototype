using EchoLib.Routing.Identification;

namespace EchoLib.Routing.Storage;

public class TargetInstanceRegistry
{
    private Dictionary<Type, ITarget> _targets = new ();
    
    public void Register<T>(Type instanceType, T instance) where T : ITarget
    {
        _targets[instanceType] = instance;
    }

    public T Get<T>() where T : ITarget
    {
        return (T) _targets[typeof(T)];
    }

    public bool TryGet(Type t, out ITarget? target)
    {
	    return _targets.TryGetValue(t, out target);
    }
}