namespace Server.JmDbConverter;

[AttributeUsage(AttributeTargets.Property)]
public sealed class BindsToAttribute(string targetProperty) : Attribute
{
    public string TargetProperty { get; } = targetProperty;
}