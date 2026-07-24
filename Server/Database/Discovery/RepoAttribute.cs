namespace Server.Database.Discovery;

[AttributeUsage(AttributeTargets.Interface)]
public class RepoAttribute(Type defaultImplementation) : Attribute
{
	public Type DefaultImplementation { get; } = defaultImplementation;
}