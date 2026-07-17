using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Protocol.Models.Params;
using EchoLib.Protocol.Models.Params.Generic;

namespace EchoLib.Transport;

public interface IMessageEndpoint
{
	public Task ErrorAsync(ProtocolException err, Guid mid);
	public Task SendAsync<T>(string target, T param, Guid mid) where T : IParam;
	public Task<TResponse> RequestAsync<TResponse, TParam>(string target, TParam param) where TParam : IParam;
}