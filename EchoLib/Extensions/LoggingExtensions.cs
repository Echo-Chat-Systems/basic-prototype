using EchoLib.Routing;
using Microsoft.Extensions.Logging;

namespace EchoLib.Extensions;

public static class LoggingExtensions
{
	public static LogLevel ReceiveLevel = LogLevel.Debug;

	private const string Template = "[{Mid}] [{Target}:{Action}] {Message}";

	public static void LogMessage(this ILogger logger, LogLevel level, RoutingContext ctx, string? message)
	{
		logger.Log(level, Template, ctx.MessageId, ctx.OriginalMessage.Target, ctx.OriginalMessage.Data.Action, message);
	}

	public static void MessageDebug(this ILogger logger, RoutingContext ctx, string? message)
	{
		logger.LogMessage(LogLevel.Debug, ctx, message);
	}

	public static void MessageError(this ILogger logger, RoutingContext ctx, string? message)
	{
		logger.LogMessage(LogLevel.Error, ctx, message);
	}
}