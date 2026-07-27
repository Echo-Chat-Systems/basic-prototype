using System.Collections.Generic;
using System.Threading.Tasks;
using EchoLib.Models.Data.Guild;
using EchoLib.Models.Params.Guilds;
using EchoLib.Routing.Identification;
using EchoLib.Transport;

namespace AvaloniaClient.Targets;

public class GuildsTarget
(
	AppState state
) : ITarget
{
	public string Name => "guilds";

	private IMessageEndpoint Endpoint => state.Net.Client.BuildEndpoint;

	public async Task<IEnumerable<JGuild>> Query()
	{
		return (await Endpoint.RequestAsync<GuildQueryResponseParams, GuildQueryParams>(Name, new GuildQueryParams())).Guilds;
	}
}