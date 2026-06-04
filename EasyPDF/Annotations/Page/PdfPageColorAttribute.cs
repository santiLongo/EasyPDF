namespace EasyPDF.Annotations.Page;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PdfPageColorAttribute : Attribute
{
    public string Color { get; }

    public PdfPageColorAttribute(string color) => Color = color;
}
