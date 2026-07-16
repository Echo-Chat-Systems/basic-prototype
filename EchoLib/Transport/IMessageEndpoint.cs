using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;
using EchoLib.Protocol.Models.Params.Generic;

namespace EchoLib.Transport;

public interface IMessageEndpoint
{
	public Task ErrorAsync(ProtocolException err, Guid? mid = null);
	public Task SendAsync<T>(string target, string action, T param, Guid? mid = null);
	public Task<TResponse> RequestAsync<TResponse, TParam>(string target, string action, TParam param);
}