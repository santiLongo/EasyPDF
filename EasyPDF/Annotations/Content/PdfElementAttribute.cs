namespace EasyPDF.Annotations.Content;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public abstract class PdfElementAttribute : Attribute
{
    public int Order { get; set; } = 0;
}
