using Microsoft.Extensions.Logging;

namespace EchoLib.Routing.Identification;

public abstract class TargetBase<T> : ITarget
{
	public abstract string Name { get; }

	protected readonly ILogger<T> Logger;

	protected TargetBase(ILogger<T> logger)
	{
		Logger = logger;
	}
}