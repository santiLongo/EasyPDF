namespace EasyPDF.Annotations.Content;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfTextAttribute : PdfElementAttribute
{
    public string? Label { get; set; }
}
