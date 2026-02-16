namespace EchoLib.Core.Routing;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ActionHandlerAttribute(string actionName) : Attribute
{
	public string ActionName { get; } = actionName;
}