using System.Reflection;
using EchoLib.Routing.Identification;

namespace EchoLib.Routing.Discovery;

public static class ImplementationFinder
{
	public static Type? Find<T>()
	{
		// ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
		return Assembly.GetEntryAssembly()?
			.GetTypes()
			.Where(t => typeof(T).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
			.FirstOrDefault();
	}
}