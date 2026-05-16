namespace Server.JmDbConverter;

[AttributeUsage(AttributeTargets.Class)]
public sealed class BindsToModelAttribute(Type targetType) : Attribute
{
    public Type TargetType { get; } = targetType;
}