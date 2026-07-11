using EchoLib.Protocol;
using EchoLib.Protocol.Exceptions;

namespace EchoLib.Transport;

public interface IMessageEndpoint
{
    public Task SendErrorAsync(ProtocolException ex, Envelope context);
    public Task SendResponseAsync(Guid messageId, Envelope data);
    public Task SendMessageAsync(Envelope data);
}