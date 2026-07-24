using System.Reflection;
using EchoLib.Routing.Identification;

namespace EchoLib.Routing.Discovery;

public static class TargetHubFinder
{
	public static Type? Find()
	{
		// ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
		return Assembly.GetEntryAssembly()?
			.GetTypes()
			.Where(t => typeof(ITargetHub).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
			.FirstOrDefault();
	}
}