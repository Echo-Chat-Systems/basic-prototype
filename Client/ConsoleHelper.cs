namespace Client;

public static class ConsoleHelper
{
	/// <summary>
	/// Helper function to get a valid console line input.
	/// </summary>
	/// <param name="prompt">User prompt text.</param>
	/// <param name="invalidResponse">The response printed if there is no valid input</param>
	/// <returns>User input.</returns>
	public static string GetConsoleInput(string prompt, string invalidResponse = "Invalid input, please try again.")
	{
		string? input = null;

		// Loop until input is not null
		while (true)
		{
			// Prompt user
			Console.Write(prompt);

			// Read input
			input = Console.ReadLine();

			// If the input is not null, return it
			if (input is not null) return input;

			Console.WriteLine(invalidResponse);
		}
	}
}