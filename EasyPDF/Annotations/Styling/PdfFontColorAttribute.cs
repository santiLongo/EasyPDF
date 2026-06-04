namespace EasyPDF.Annotations.Styling;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfFontColorAttribute : Attribute
{
    public string Color { get; }

    public PdfFontColorAttribute(string color) => Color = color;
}
