namespace EchoLib.Protocol.Exceptions;

public delegate void ProtocolExceptionCallback(string message, object[] parameters);

public static class ExceptionCallbackRegistry
{
    private static Dictionary<string, List<ProtocolExceptionCallback>> Callbacks { get; } = new();

    public static void RegisterCallback(string name, ProtocolExceptionCallback callback)
    {
        if (!Callbacks.ContainsKey(name)) Callbacks[name] = [];
        
        Callbacks[name].Add(callback);
    }
    
    public static void RegisterCallback<T>(ProtocolExceptionCallback callback) where T : ProtocolException
    {
        if (!Callbacks.ContainsKey(typeof(T).Name)) Callbacks[typeof(T).Name] = [];
        
        Callbacks[typeof(T).Name].Add(callback);
    }

    public static List<ProtocolExceptionCallback> Get(string name)
    {
        return Callbacks[name];
    }
    
    public static List<ProtocolExceptionCallback> Get<T>() where T : ProtocolException
    {
        return Callbacks[typeof(T).Name];
    }
}