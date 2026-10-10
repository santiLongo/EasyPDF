using EasyPDF.Interfaces;
using EasyPDF.Sample.Models;
using EasyPDF.Sample.Services.Interface;

namespace EasyPDF.Sample.Services;

public class InvoiceServices : IGenerarReporte
{
    private IPdfGenerator _generator;

    public InvoiceServices(IPdfGenerator generator)
    {
        _generator = generator;
    }

    public string Generar()
    {
        var invoice = new InvoiceDocument
        {
            Title = "FACTURA",
            Subtitle = "Empresa Ejemplo S.A. | CUIT: 30-12345678-9",
            InvoiceNumber = "FAC-2024-00042",
            Date = DateTime.Now.ToString("dd/MM/yyyy"),
            CustomerName = "Juan Pérez",
            CustomerAddress = "Av. Corrientes 1234, CABA, Argentina",
            Items =
            [
                new InvoiceItem { ProductName = "Desarrollo web", Quantity = 40, UnitPrice = "$150.00", Subtotal = "$6,000.00" },
                new InvoiceItem { ProductName = "Diseño UI/UX", Quantity = 20, UnitPrice = "$120.00", Subtotal = "$2,400.00" },
                new InvoiceItem { ProductName = "Consultoría técnica", Quantity = 5, UnitPrice = "$200.00", Subtotal = "$1,000.00" },
                new InvoiceItem { ProductName = "Soporte mensual", Quantity = 1, UnitPrice = "$500.00", Subtotal = "$500.00" },
            ],
            Total = "$9,900.00",
            PaymentCondition = "Contado al vencimiento (30 días)"
        };
        
        var fileName = "factura.pdf";
        _generator.GenerateToFile(invoice, fileName, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        return fileName;
    }
}