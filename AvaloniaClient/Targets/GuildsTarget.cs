using System.Collections.Generic;
using System.Threading.Tasks;
using EchoLib.Models.Data.Guild;
using EchoLib.Models.Params.Guilds;
using EchoLib.Routing.Identification;
using EchoLib.Transport;
using EchoLib.Core.Snowflake;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.Targets;

public class GuildsTarget(
	ILogger<GuildsTarget> logger,
	AppState state
) : TargetBase<GuildsTarget>(logger)
{
	public override string Name => "guilds";

	private IMessageEndpoint Endpoint => state.Net.Client.BuildEndpoint;

	public async Task<JGuild?> Get(Snowflake id)
	{
		return (await Endpoint.RequestAsync<GuildGetResponseParams, GuildGetParams>(Name, new GuildGetParams
		{
			Id = id
		})).Guild;
	}

	public async Task<IEnumerable<JGuild>> Query()
	{
		return (await Endpoint.RequestAsync<GuildQueryResponseParams, GuildQueryParams>(Name, new GuildQueryParams())).Guilds;
	}

	public async Task<Snowflake> New(string name)
	{
		return (await Endpoint.RequestAsync<GuildCreateResponseParams, GuildCreateParams>(Name, new GuildCreateParams
		{
			Name = name
		})).Id;
	}
}