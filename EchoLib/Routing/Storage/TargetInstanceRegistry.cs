using EchoLib.Routing.Identification;

namespace EchoLib.Routing.Storage;

public class TargetInstanceRegistry
{
    private Dictionary<Type, ITarget> _targets = new ();
    
    public void Register<T>(T instance) where T : ITarget
    {
        _targets[typeof(T)] = instance;
    }

    public T Get<T>() where T : ITarget
    {
        return (T) _targets[typeof(T)];
    }
}