using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.User;
using Server.JmDbConverter;

namespace Server.Database.Models.Public;

[BindsToModel(typeof(JUserModel))]
public class UserDbm
{
	public required PublicSigningKey Id { get; init; }
	public required PublicEncryptionKey Ek { get; init; }
	public required DateTime CreatedAt { get; init; }

}