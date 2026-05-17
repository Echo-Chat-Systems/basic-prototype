namespace Server.JmDbConverter;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class MapsToAttribute(Type targetType, string targetProperty) : Attribute
{
    public Type TargetType { get; } = targetType;
    public string TargetProperty { get; } = targetProperty;
}