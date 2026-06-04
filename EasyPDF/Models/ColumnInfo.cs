using System.Reflection;

namespace EasyPDF.Models;

public sealed class ColumnInfo
{
    public required PropertyInfo Property { get; init; }
    public string? Header { get; init; }
    public float RelativeWidth { get; init; } = 1f;
    public int Order { get; init; }
    public IReadOnlyList<Attribute> Attributes { get; init; } = Array.Empty<Attribute>();
}
