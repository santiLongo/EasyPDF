namespace EasyPDF.Annotations.Page;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PdfDefaultFontSizeAttribute : Attribute
{
    public float Size { get; }

    public PdfDefaultFontSizeAttribute(float size) => Size = size;
}
