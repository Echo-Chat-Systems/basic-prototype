using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.User;
using Microsoft.EntityFrameworkCore;

namespace Server.Database.Models.Public;

[PrimaryKey(nameof(Id))]
public class User
{
	public required PublicSigningKey Id { get; init; }
	public required PublicEncryptionKey Ek { get; init; }
	public required DateTime CreatedAt { get; init; }
	public required string Username { get; set; }
	public required short Tag { get; set; }
	public required JProfile Profile { get; set; }
	public required string Settings { get; set; }  // Settings is a client-side encrypted blob
	public DateTime? LastOnline { get; set; }
	public required bool IsOnline { get; set; }
	public required bool IsBanned { get; set; }
}