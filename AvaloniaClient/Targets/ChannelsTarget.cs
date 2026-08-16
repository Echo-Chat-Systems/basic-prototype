using System.Threading.Tasks;
using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Params.Channels;
using EchoLib.Routing.Identification;
using EchoLib.Transport;
using Microsoft.Extensions.Logging;

namespace AvaloniaClient.Targets;

public class ChannelsTarget(
	ILogger<ChannelsTarget> logger,
	AppState state
) : TargetBase<ChannelsTarget>(logger)
{
	public override string Name => "channels";

	private IMessageEndpoint Endpoint => state.Net.Client.BuildEndpoint;

	public async Task<JChannel?> Get(Snowflake id)
	{
		return (await Endpoint.RequestAsync<ChannelGetResponseParams, ChannelGetParams>(Name, new ChannelGetParams() { Id = id })).Channel;
	}

	public async Task<Snowflake> New(Snowflake guild, string name)
	{
		return (await Endpoint.RequestAsync<ChannelCreateResponseParams, ChannelCreateParams>(Name, new ChannelCreateParams
		{
			Name = name,
			Guild = guild
		})).Id;
	}
}