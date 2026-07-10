namespace EchoLib.Core.Routing;

[AttributeUsage(AttributeTargets.Method)]
public class RouteAttribute(string actionName) : Attribute
{
	public string ActionName { get; } = actionName;
}