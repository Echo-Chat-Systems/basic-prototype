using EchoLib.Core;
using EchoLib.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;
using Terminal.Gui.App;
using TuiClient;
using TuiClient.Targets;
using TuiClient.Windows;


public class Program
{
	// Set up service provider
	/*public static readonly IServiceProvider Services = new ServiceCollection()
		.AddLogging(builder =>
		{
			builder.AddConsole();
			builder.SetMinimumLevel(LogLevel.Debug);

		})
		// Backend side
		.AddRouting()
		.AddSingleton<TargetCollection>()

		// GUI
		.AddTui()
		.BuildServiceProvider();
*/
	public static  void Main()
	{
		// Create app
        //TerminalApplication app = Services.GetRequiredService<TerminalApplication>();
        //await app.RunAsync();

        using var app = Application.Create();
        app.Init();
        app.Run<GuiBootstrapper>().GetResult();
	}
}



