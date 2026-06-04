namespace EasyPDF.Annotations.Content;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfImageAttribute : PdfElementAttribute
{
    public float? Width { get; set; }
    public float? Height { get; set; }
}
