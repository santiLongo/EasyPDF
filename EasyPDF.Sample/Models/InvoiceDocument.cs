using EasyPDF.Annotations.Content;
using EasyPDF.Annotations.Page;
using EasyPDF.Annotations.Styling;
using EasyPDF.Enums;
using EasyPDF.Interfaces;

namespace EasyPDF.Sample.Models;

[PdfPageSize(PdfPageSizeType.A4)]
[PdfPageMargin(2f, PdfUnit.Centimetre)]
[PdfPageColor("#FFFFFF")]
[PdfDefaultFontSize(11f)]
public sealed class InvoiceDocument : IPdfDocument
{
    [PdfTitle(Order = 0)]
    [PdfBold]
    [PdfFontSize(30f)]
    [PdfFontColor("#1E3A5F")]
    [PdfAlignment(PdfAlignment.Center)]
    [PdfPadding(16f)]
    [PdfBackgroundColor("#EEF4FB")]
    public string Title { get; set; } = "FACTURA";

    [PdfSubtitle(Order = 1)]
    [PdfFontColor("#555555")]
    [PdfAlignment(PdfAlignment.Center)]
    [PdfSpacing(0f, 8f)]
    public string Subtitle { get; set; } = string.Empty;

    [PdfSeparator(Order = 2)]
    public string? Separator1 { get; set; }

    [PdfText(Order = 3, Label = "N° Factura:")]
    [PdfFontSize(12f)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [PdfText(Order = 4, Label = "Fecha:")]
    [PdfFontSize(12f)]
    public string Date { get; set; } = string.Empty;

    [PdfText(Order = 5, Label = "Cliente:")]
    [PdfBold]
    [PdfFontSize(13f)]
    [PdfFontColor("#1E3A5F")]
    [PdfSpacing(8f, 4f)]
    public string CustomerName { get; set; } = string.Empty;

    [PdfText(Order = 6, Label = "Dirección:")]
    public string CustomerAddress { get; set; } = string.Empty;

    [PdfSeparator(Order = 7)]
    public string? Separator2 { get; set; }

    [PdfTable(Order = 8,
        Title = "Detalle de items",
        ShowHeader = true,
        StripedRows = true,
        HeaderBackgroundColor = "#1E3A5F",
        HeaderFontColor = "#FFFFFF",
        StripedRowColor = "#F0F4FA")]
    [PdfSpacing(8f, 12f)]
    public List<InvoiceItem> Items { get; set; } = new();

    [PdfSeparator(Order = 9)]
    public string? Separator3 { get; set; }

    [PdfText(Order = 10, Label = "TOTAL:")]
    [PdfBold]
    [PdfFontSize(16f)]
    [PdfFontColor("#C0392B")]
    [PdfAlignment(PdfAlignment.Right)]
    [PdfSpacing(6f, 0f)]
    public string Total { get; set; } = string.Empty;

    [PdfText(Order = 11, Label = "Condición de pago:")]
    [PdfItalic]
    [PdfFontColor("#666666")]
    [PdfAlignment(PdfAlignment.Right)]
    public string PaymentCondition { get; set; } = string.Empty;
}
