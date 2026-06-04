using System.Reflection;

namespace EasyPDF.Models;

public sealed class PropertyDescriptor
{
    private readonly IReadOnlyList<Attribute> _attributes;

    public PropertyInfo Property { get; }
    public object? Value { get; }
    public IReadOnlyList<Attribute> AllAttributes => _attributes;

    public PropertyDescriptor(PropertyInfo property, object? value, IEnumerable<Attribute> attributes)
    {
        Property = property;
        Value = value;
        _attributes = attributes.ToList().AsReadOnly();
    }

    public T? GetAttribute<T>() where T : Attribute =>
        _attributes.OfType<T>().FirstOrDefault();

    public IEnumerable<T> GetAttributes<T>() where T : Attribute =>
        _attributes.OfType<T>();

    public bool HasAttribute<T>() where T : Attribute =>
        _attributes.OfType<T>().Any();
}
