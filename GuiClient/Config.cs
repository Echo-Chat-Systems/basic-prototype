using EchoLib.Configuration.Attributes;

namespace GuiClient;

public class Config
{
	[ConfigProperty]
	public PersistenceModel Persistence { get; init; }

	[ConfigModel]
	public class PersistenceModel
	{
		public required string EchoDirectory { get; init; }
	}

	[ConfigModel]
	public class AppearanceModel
	{
		public required string ColourTheme { get; init; }
	}

}