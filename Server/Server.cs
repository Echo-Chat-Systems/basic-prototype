using WebSocketSharp;
using WebSocketSharp.Server;

namespace Server;

public class TestService : WebSocketBehavior
{
	protected override void OnMessage(MessageEventArgs e)
	{
		Console.WriteLine($"Message received: {e.Data}");

		Sessions.Broadcast(e.Data);
	}
}

public class Server
{
	public static readonly WebSocketServer Socket = new("ws://localhost:69");

	public static async void Run()
	{
		Socket.AddWebSocketService<TestService>("/");
		Socket.Start();
		Console.WriteLine("Server is running. Press any key to stop...");
		Console.ReadKey();
		Socket.Stop();
	}
}