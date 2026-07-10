namespace EchoLib.Core.Routing.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ActionHandlerAttribute(string actionName) : Attribute
{
	public string ActionName { get; } = actionName;
}