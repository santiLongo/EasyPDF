namespace EasyPDF.Annotations.Styling;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfBackgroundColorAttribute : Attribute
{
    public string Color { get; }

    public PdfBackgroundColorAttribute(string color) => Color = color;
}
