using EchoLib.Core.Crypto.Signing;
using EchoLib.Models.Data;
using Server.JmDbConverter;

namespace Server.Database.Models.Public;

[BindsToModel(typeof(JUserModel))]
public class UserDbm
{
    [MapsTo(typeof(JUserModel), nameof(JUserModel.Id))]
    public required UserId Id { get; init; }
    
}