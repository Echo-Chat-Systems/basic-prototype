using System.Reflection;

namespace EchoLib.Core.Routing.Exceptions;

public static class ExceptionsRegistry
{
	public static readonly Dictionary<string, Type> Exceptions;

	static ExceptionsRegistry()
	{
		Exceptions = Assembly.GetEntryAssembly()!
			.GetTypes()
			.Where(t =>
				typeof(ProtocolException).IsAssignableFrom(t) &&
				t is { IsAbstract: false }
			)
			.ToDictionary(t =>
			{
				ProtocolException ins = (ProtocolException)Activator.CreateInstance(t)!;
				return ins.Message;
			}, t => t);
	}
}