namespace Server;

internal class Program
{
	private static void Main(string[] args)
	{
		Server server = new();

		try
		{
			Task serverProcess = server.Run();

			// Wait for server to terminate
			serverProcess.Wait();
		}
		catch (Exception e)
		{
			Console.WriteLine($"SERVER CRASHED");
			Console.WriteLine(e);
		}
	}
}