using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.User;

namespace Server.Database.Models.Public;

public class UserDbm
{
	public required PublicSigningKey Id { get; init; }
	public required PublicEncryptionKey Ek { get; init; }
	public required DateTime CreatedAt { get; init; }
	public required string Username { get; set; }
	public required short Tag { get; set; }

	public class New
	{
		public required PublicSigningKey Id { get; init; }
		public required PublicEncryptionKey Ek { get; init; }
		public required string Username { get; init; }
		public required short Tag { get; init; }
		public JProfile Profile { get; set; } = null;
	}
}