using WebSocketSharp;

namespace Server;

internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Starting server...");
		Server.Run();
	}
}