using Microsoft.Extensions.Logging;

namespace EchoLib.Transport;

public static class TaskExtensions
{
	public static async void Forget(this Task task, ILogger logger)
	{
		try
		{
			await task;
		}
		catch (Exception ex)
		{
			logger.LogCritical(ex, "Unhandled async exception in fire-and-forget");
		}
	}
}