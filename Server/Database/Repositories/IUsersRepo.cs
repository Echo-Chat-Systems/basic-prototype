using EchoLib.Crypto.Signing;
using Server.Database.Discovery;
using Server.Database.Models.Public;
using Server.Database.Repositories.Impl;

namespace Server.Database.Repositories;

[Repo(typeof(PgUsersRepo))]
public interface IUsersRepo
{
	UserDbm? Get(PublicSigningKey id);
	Task<UserDbm?> GetAsync(PublicSigningKey id);

	UserDbm Insert(UserDbm.New user);
	Task<UserDbm> InsertAsync(UserDbm.New user);
}