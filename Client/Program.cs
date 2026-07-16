namespace Client;

public class Program
{
	private static async Task Main(string[] args)
	{
		Client client = new();
		await client.Run();
	}
}