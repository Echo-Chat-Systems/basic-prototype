using EchoLib.Crypto.Signing;

namespace EchoLib.Protocol.Models.Data;

public class JUserModel
{
	public required PublicSigningKey Id { get; init; }
	public required DateTime CreatedAt { get; init; }
}