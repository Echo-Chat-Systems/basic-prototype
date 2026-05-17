namespace Server.JmDbConverter;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class BindsToModelAttribute(Type targetType) : Attribute
{
    public Type TargetType { get; } = targetType;
}