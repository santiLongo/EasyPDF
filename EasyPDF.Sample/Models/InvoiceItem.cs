using EasyPDF.Annotations.Table;

namespace EasyPDF.Sample.Models;

public sealed class InvoiceItem
{
    [PdfColumn("Producto", Order = 0, RelativeWidth = 3f)]
    public string ProductName { get; set; } = string.Empty;

    [PdfColumn("Cantidad", Order = 1, RelativeWidth = 1f)]
    public int Quantity { get; set; }

    [PdfColumn("Precio unitario", Order = 2, RelativeWidth = 2f)]
    public string UnitPrice { get; set; } = string.Empty;

    [PdfColumn("Subtotal", Order = 3, RelativeWidth = 2f)]
    public string Subtotal { get; set; } = string.Empty;

    [PdfColumnIgnore]
    public Guid InternalId { get; set; } = Guid.NewGuid();
}
