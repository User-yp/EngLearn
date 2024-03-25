namespace EventBus;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EventNameAttribute(string name) : Attribute
{
    public string Name { get; init; } = name;
}
/*public class EventNameAttribute : Attribute
{
    public EventNameAttribute(string name)
    {
        this.Name = name;
    }
    public string Name { get; init; }
}*/