using EchoLib.Crypto.Signing;
using EchoLib.Protocol.Models.Data;
using Server.JmDbConverter;

namespace Server.Database.Models.Public;

[BindsToModel(typeof(JUserModel))]
public class UserDbm
{
	[MapsTo(typeof(JUserModel), nameof(JUserModel.Id))]
	public required PublicSigningKey Id { get; init; }

	public required DateTime CreatedAt { get; init; }

}