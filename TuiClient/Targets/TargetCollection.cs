using Microsoft.Extensions.DependencyInjection;

namespace TuiClient.Targets;

public class TargetCollection(
	AuthTarget authTarget
)
{
	public AuthTarget Auth { get; } = authTarget;
}

public static class TargetCollectionExtensions
{
	public static IServiceCollection AddTargets(this IServiceCollection services)
	{
		return services
			.AddSingleton<TargetCollection>()
			.AddSingleton<AuthTarget>();
	}
}