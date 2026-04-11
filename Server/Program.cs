using WebSocketSharp;

namespace Server;

internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Starting server...");
		new Server().Run();
	}
}