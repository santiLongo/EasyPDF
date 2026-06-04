namespace EasyPDF.Annotations.Content;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfSeparatorAttribute : PdfElementAttribute
{
    public float Thickness { get; set; } = 1f;
    public string Color { get; set; } = "#CCCCCC";
    public float VerticalSpacing { get; set; } = 8f;
}
