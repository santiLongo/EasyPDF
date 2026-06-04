namespace EasyPDF.Annotations.Content;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfTableAttribute : PdfElementAttribute
{
    public string? Title { get; set; }
    public bool ShowHeader { get; set; } = true;
    public bool StripedRows { get; set; } = true;
    public string HeaderBackgroundColor { get; set; } = "#4472C4";
    public string HeaderFontColor { get; set; } = "#FFFFFF";
    public string StripedRowColor { get; set; } = "#F2F2F2";
}
