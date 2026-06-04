using EasyPDF.Enums;

namespace EasyPDF.Models;

public sealed class PageConfiguration
{
    public PdfPageSizeType PageSize { get; set; } = PdfPageSizeType.A4;
    public string PageColor { get; set; } = "#FFFFFF";
    public float MarginValue { get; set; } = 2f;
    public PdfUnit MarginUnit { get; set; } = PdfUnit.Centimetre;
    public float DefaultFontSize { get; set; } = 11f;
}
