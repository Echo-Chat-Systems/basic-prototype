using EchoLib.Core;

namespace EchoLib.Models.Params.Guilds;

public class GuildQueryParams : IParam
{
	public string Action => RouteNames.Guilds.Query;
}