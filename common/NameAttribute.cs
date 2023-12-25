namespace AoC;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class NameAttribute(string name) : Attribute
{
	public string Name { get; } = name;
}
