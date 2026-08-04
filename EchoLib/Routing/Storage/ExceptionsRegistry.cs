using System.Reflection;
using EchoLib.Protocol.Exceptions;

namespace EchoLib.Routing.Storage;

public static class ExceptionsRegistry
{
	public static Dictionary<string, Type> Exceptions { get; private set; }

	public static void Find()
	{
		Exceptions = Assembly.GetEntryAssembly()!.GetTypes().Where(t =>
				typeof(ProtocolException).IsAssignableFrom(t) &&
				t is { IsAbstract: false }
			)
			.Concat(
				Assembly.GetExecutingAssembly().GetTypes().Where(t =>
					typeof(ProtocolException).IsAssignableFrom(t) &&
					t is { IsAbstract: false }))
			.ToDictionary(t =>
			{
				ProtocolException ins = (ProtocolException)Activator.CreateInstance(t)!;
				return ins.Message;
			}, t => t);
	}
}