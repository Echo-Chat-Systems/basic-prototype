namespace EchoLib.Core.Routing;

[AttributeUsage(AttributeTargets.Method)]
public class ActionHandlerAttribute(string actionName) : Attribute
{
	public string ActionName { get; } = actionName;
}