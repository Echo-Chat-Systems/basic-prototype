using EchoLib.Crypto.Signing;
using Server.Database.Discovery;
using Server.Database.Models.Chat;
using Server.Database.Models.Public;
using Server.Database.Repositories.Impl;

namespace Server.Database.Repositories;

[Repo(typeof(PgUsersRepo))]
public interface IUsersRepo : IRepo<UserDbm, PublicSigningKey, UserDbm.New>
{

}