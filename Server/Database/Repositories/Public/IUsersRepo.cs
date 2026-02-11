using EchoLib.Core;
using Models.Functional.Crypto.Signing;
using Server.Database.Models.Public;

namespace Server.Database.Repositories.Public;

public interface IUsersRepo : IRepo
{
	UserDbm Get(UserId id);
	Task<UserDbm> GetAsync(UserId id);
}