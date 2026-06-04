namespace EasyPDF.Annotations.Styling;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfPaddingAttribute : Attribute
{
    public float Top { get; }
    public float Bottom { get; }
    public float Left { get; }
    public float Right { get; }

    public PdfPaddingAttribute(float all)
    {
        Top = Bottom = Left = Right = all;
    }

    public PdfPaddingAttribute(float vertical, float horizontal)
    {
        Top = Bottom = vertical;
        Left = Right = horizontal;
    }

    public PdfPaddingAttribute(float top, float bottom, float left, float right)
    {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }
}
