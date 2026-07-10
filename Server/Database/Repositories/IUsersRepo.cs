using EchoLib.Core.Crypto.Signing;
using Server.Database.Models.Public;

namespace Server.Database.Repositories;

public interface IUsersRepo
{
	UserDbm? Get(PublicSigningKey id);
	Task<UserDbm?> GetAsync(PublicSigningKey id);
}