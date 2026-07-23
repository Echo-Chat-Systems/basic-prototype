using EchoLib.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TuiClient.Targets;

namespace TuiClient;

public class Program
{
	// Set up service provider
	public static readonly IServiceProvider Services = new ServiceCollection()
	.AddLogging(builder =>
	{
		builder.AddDebug();
		builder.SetMinimumLevel(LogLevel.Debug);

	})
	// Backend side
	.AddRouting()
	.AddTargets()

	// GUI
	.AddTui()
	.BuildServiceProvider();

	public static async Task Main()
	{
		TerminalApplication app = Services.GetRequiredService<TerminalApplication>();
		try
		{
			// Create app
			await app.RunAsync();
		}
		finally
		{
			await app.Stop();
			await app.Dispose();
		}

	}
}