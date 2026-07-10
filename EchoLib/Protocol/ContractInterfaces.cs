namespace EchoLib.Protocol;

public interface IMessage
{
	
}

public interface IRequest<TResponse> : IMessage
{

}

public interface INotification : IMessage
{

}