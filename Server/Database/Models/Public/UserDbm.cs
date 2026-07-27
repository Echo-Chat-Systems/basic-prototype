using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.User;

namespace Server.Database.Models.Public;

public class UserDbm : BaseDbm<UserDbm.New>
{
	public new required PublicSigningKey Id { get; init; }
	public required PublicEncryptionKey Ek { get; init; }
	public required DateTime CreatedAt { get; init; }
	public required string Username { get; set; }
	public required short Tag { get; set; }
	public required JProfile Profile { get; set; }
	public required string Settings { get; set; }  // Settings is a client-side encrypted blob
	public DateTime? LastOnline { get; set; }
	public required bool IsOnline { get; set; }
	public required bool IsBanned { get; set; }

	public class New : NewBase
	{
		public new required PublicSigningKey Id { get; init; }
		public required PublicEncryptionKey Ek { get; init; }
		public required string Username { get; init; }
		public required short Tag { get; init; }
		public required JProfile Profile { get; set; }
		public string Settings { get; set; } = "";
		public DateTime? LastOnline { get; set; }
		public bool IsOnline { get; set; } = false;
		public bool IsBanned { get; set; } = false;
	}
}