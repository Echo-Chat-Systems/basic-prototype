using WebSocketSharp;

namespace Server;

class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("Starting server...");
		Server.Run();
	}
}