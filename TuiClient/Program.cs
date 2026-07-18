using EchoLib.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;
using Terminal.Gui.App;
using TuiClient;
using TuiClient.Windows;

// Set up service provider
IServiceProvider services = new ServiceCollection()
	.AddLogging(builder =>
	{
		builder.AddConsole();
		builder.SetMinimumLevel(LogLevel.Debug);

	})
	// Backend side
	.AddRouting()
	.AddSingleton<ApplicationState>()
	.AddSingleton<Targets>()
	// GUI
	.AddSingleton<IApplication>(_ => Application.Create().Init())
	.AddSingleton<WindowManager>()
	.BuildServiceProvider();

// Create app
IApplication gui = services.GetRequiredService<IApplication>();




// Dispose of app cleanly and exit
gui.Dispose();



