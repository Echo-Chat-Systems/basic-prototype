using EchoLib.Core.Snowflake;
using Server.Database.Discovery;
using Server.Database.Models.Chat;
using Server.Database.Repositories.Impl;

namespace Server.Database.Repositories;

[Repo(typeof(PgChannelsRepo))]
public interface IChannelsRepo : IRepo<ChannelDbm, Snowflake, ChannelDbm.New>
{

}