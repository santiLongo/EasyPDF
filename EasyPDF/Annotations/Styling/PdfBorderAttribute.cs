namespace EasyPDF.Annotations.Styling;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfBorderAttribute : Attribute
{
    public float Thickness { get; }
    public string Color { get; }

    public PdfBorderAttribute(float thickness = 1f, string color = "#000000")
    {
        Thickness = thickness;
        Color = color;
    }
}
