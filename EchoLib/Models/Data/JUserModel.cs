using EchoLib.Core.Crypto.Signing;

namespace EchoLib.Models.Data;

public class JUserModel
{
	public required PublicSigningKey Id { get; init; }
	public required DateTime CreatedAt { get; init; }
}