using EchoLib.Core;
using EchoLib.Models.Params.Guilds;
using EchoLib.Routing;
using EchoLib.Routing.Identification;
using Microsoft.Extensions.Logging;
using Server.Database;
using Server.Database.Models.Chat;

namespace Server.Targets;

public class GuildsTarget(
	ILogger<GuildsTarget> logger,
	DbHub hub
)
	: ITarget
{
	public string Name => "guilds";

	[Route("create")]
	public async Task Create(RoutingContext ctx, GuildCreateParams para)
	{
	}

	[Route("delete")]
	public async Task Delete(RoutingContext ctx, GuildDeleteParams para)
	{
	}

	[Route("get")]
	public async Task Get(RoutingContext ctx, GuildGetParams para)
	{
	}

	[Route("query")]
	public async Task Query(RoutingContext ctx, GuildQueryParams para)
	{
	}
}