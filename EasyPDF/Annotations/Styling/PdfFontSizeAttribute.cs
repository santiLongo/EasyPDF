namespace EasyPDF.Annotations.Styling;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfFontSizeAttribute : Attribute
{
    public float Size { get; }

    public PdfFontSizeAttribute(float size) => Size = size;
}
