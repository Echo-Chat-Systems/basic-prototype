using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;

namespace EchoLib.Models.Data.User;

public class JUserModel
{
	public required PublicSigningKey Id { get; init; }
	public required PublicEncryptionKey Ek { get; init; }
	public required DateTime CreatedAt { get; init; }
}