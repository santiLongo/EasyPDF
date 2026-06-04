namespace EasyPDF.Annotations.Table;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfColumnAttribute : Attribute
{
    public string? Header { get; set; }
    public int Order { get; set; } = 0;
    public float RelativeWidth { get; set; } = 1f;

    public PdfColumnAttribute() { }

    public PdfColumnAttribute(string header) => Header = header;
}
