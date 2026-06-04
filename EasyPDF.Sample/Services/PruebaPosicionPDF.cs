using EasyPDF.Builder;
using EasyPDF.Enums;
using EasyPDF.Sample.Models.PosicionCliente;
using EasyPDF.Sample.Services.Interface;

namespace EasyPDF.Sample.Services;

public class PruebaPosicionPDF : IGenerarReporte
{
    public string Generar()
    {
        var data = BuildData();

        var bytes = PdfDocumentBuilder.Create()
            .Page(p => p
                .A4()                   // A4 landscape
                .Margin(1.2f, PdfUnit.Centimetre)
                .DefaultFontSize(7.5f))

            // ── Header (repite en cada página) ────────────────────────────
            .Header(h =>
            {
                // Fila 1: empresa | título centrado | fecha+hora
                h.Row(r =>
                {
                    r.Auto(col => col.Text(data.Empresa).Bold().FontSize(10));

                    r.Fill(col =>
                    {
                        col.Text(data.Titulo).Bold().FontSize(15).Underline().AlignCenter();
                        col.Text(data.Periodos).Bold().FontSize(9).AlignCenter();
                    });

                    r.Auto(col =>
                    {
                        col.Text(data.Fecha).FontSize(7.5f).AlignRight();
                        col.Text(data.Hora).FontSize(7.5f).AlignRight();
                    });
                });

                // Fila 2: filtros
                h.Text($"Cliente :  {data.ClienteFilter}   Producto :  {data.ProductoFilter}")
                    .Bold().FontSize(8.5f).AlignCenter();

                // Fila 3: cabecera de columnas (mismos anchos relativos que la tabla de contenido)
                h.ColumnHeaderRow(cols =>
                {
                    cols.Column("Contrato",    1.4f);
                    cols.Column("Fecha",       1.8f);
                    cols.Column("Vendedor",    3.8f);
                    cols.Column("Comprador",   3.8f);
                    cols.Column("Kilos Ctrat", 2.2f);
                    cols.Column("Precio",      1.3f);
                    cols.Column("Puerto",      1.8f);
                    cols.Column("Período",     1.6f);
                    cols.Column("Producto",    2.6f);
                });
            })

            // ── Contenido ─────────────────────────────────────────────────
            .Content(c =>
            {
                c.ReportTable<ContratoRow>(t =>
                {
                    // Columnas con los MISMOS anchos que la cabecera de arriba
                    t.Column(r => r.Contrato,  1.4f);
                    t.Column(r => r.Fecha,     1.8f);
                    t.Column(r => r.Vendedor,  3.8f);
                    t.Column(r => r.Comprador, 3.8f, col => col.Bold());
                    t.Column(r => r.KilosCtr,  2.2f, col => col.AlignRight());
                    t.Column(r => r.Precio,    1.3f, col => col.AlignRight().Bold());
                    t.Column(r => r.Puerto,    1.8f, col => col.Bold());
                    t.Column(r => r.Periodo,   1.6f, col => col.Bold());
                    t.Column(r => r.Producto,  2.6f, col => col.Bold());

                    // Filas jerárquicas via foreach
                    foreach (var cliente in data.Clientes)
                    {
                        t.SpanRow(
                            $"Cliente : {cliente.ClienteId}   {cliente.ClienteNombre}",
                            bold: true, fontSize: 8.5f, paddingTop: 3f, paddingBottom: 3f);

                        foreach (var seccion in cliente.Secciones)
                        {
                            t.SpanRow(seccion.Nombre, bold: true, fontSize: 8f, paddingTop: 1f);

                            foreach (var periodo in seccion.Periodos)
                            {
                                foreach (var row in periodo.Rows)
                                    t.DataRow(row);

                                // Subtotal período: 3 vacías | label | monto | 4 vacías
                                t.SubtotalRow($"Total Período {periodo.Periodo}", periodo.Total,
                                    isBold: false, emptyBefore: 3);
                            }

                            t.SubtotalRow($"Total {seccion.Nombre}", seccion.Total,
                                isBold: false, emptyBefore: 3);
                        }

                        // Total neto cliente (negrita)
                        t.SubtotalRow("Total Cliente Compras vs Ventas", cliente.TotalNetoCv,
                            isBold: true, emptyBefore: 3);
                    }
                });
            })
            .GeneratePdfBytes();
        
        var downloadsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "posicion_builder.pdf");

        if (!File.Exists(downloadsPath))
            File.Create(downloadsPath).Dispose();
        
        File.WriteAllBytes(downloadsPath, bytes);

        return Path.GetFileName(downloadsPath);
    }

    // ── Datos de prueba (idénticos al PDF original) ───────────────────────────

    private static PosicionClienteData BuildData() => new()
    {
        Empresa         = "AGRIGOODS",
        Titulo          = "Posición Cliente",
        Periodos        = "Periodos Junio-Julio",
        Fecha           = "02/06/2026",
        Hora            = "10:27",
        ClienteFilter   = "Clientes Seleccionados",
        ProductoFilter  = "",
        Clientes =
        [
            new ClienteSection
            {
                ClienteId     = "25,969",
                ClienteNombre = "COFCO RESOURCES S.A.",
                TotalNetoCv   = "-14,000,000",
                Secciones =
                [
                    new SeccionContratos
                    {
                        Nombre = "Ventas",
                        Total  = "28,000,000",
                        Periodos =
                        [
                            new PeriodoGroup
                            {
                                Periodo = "JUN/2026",
                                Total   = "12,000,000",
                                Rows =
                                [
                                    new ContratoRow { Contrato="07714", Fecha="28/08/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="VITOL INC",                         KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08269", Fecha="03/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                       KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07858", Fecha="31/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="VITOL INC",                         KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07837", Fecha="22/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",              KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07853", Fecha="30/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                       KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08274", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                       KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08275", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="ELEMENTA FOODS CHILE S.A.",         KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08293", Fecha="06/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",              KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                ]
                            },
                            new PeriodoGroup
                            {
                                Periodo = "JUL/2026",
                                Total   = "16,000,000",
                                Rows =
                                [
                                    new ContratoRow { Contrato="08274", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                             KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08275", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="ELEMENTA FOODS CHILE S.A.",               KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08290", Fecha="06/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="MOLINOS OVERSEAS COMMODITIES S.A.",      KilosCtr="6,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08293", Fecha="06/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",                   KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07837", Fecha="22/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",                   KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07853", Fecha="30/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                            KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08269", Fecha="03/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                            KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07858", Fecha="31/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="VITOL INC",                              KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                ]
                            }
                        ]
                    },
                    new SeccionContratos
                    {
                        Nombre = "Compras",
                        Total  = "14,000,000",
                        Periodos =
                        [
                            new PeriodoGroup
                            {
                                Periodo = "JUN/2026",
                                Total   = "7,000,000",
                                Rows =
                                [
                                    new ContratoRow { Contrato="07963", Fecha="12/12/2025", Vendedor="BUNGE S.A.",                  Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08164", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08165", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08171", Fecha="30/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08057", Fecha="09/01/2026", Vendedor="BUNGE S.A.",                  Comprador="COFCO RESOURCES S.A.", KilosCtr="3,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                ]
                            },
                            new PeriodoGroup
                            {
                                Periodo = "JUL/2026",
                                Total   = "7,000,000",
                                Rows =
                                [
                                    new ContratoRow { Contrato="08057", Fecha="09/01/2026", Vendedor="BUNGE S.A.",                  Comprador="COFCO RESOURCES S.A.", KilosCtr="3,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08164", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08165", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08171", Fecha="30/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07963", Fecha="12/12/2025", Vendedor="BUNGE S.A.",                  Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                ]
                            }
                        ]
                    }
                ]
            },
            new ClienteSection
            {
                ClienteId     = "8232",
                ClienteNombre = "BUNGE S.A.",
                TotalNetoCv   = "-14,000,000",
                Secciones =
                [
                    new SeccionContratos
                    {
                        Nombre = "Ventas",
                        Total  = "28,000,000",
                        Periodos =
                        [
                            new PeriodoGroup
                            {
                                Periodo = "JUN/2026",
                                Total   = "12,000,000",
                                Rows =
                                [
                                    new ContratoRow { Contrato="07714", Fecha="28/08/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="VITOL INC",                         KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08269", Fecha="03/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                       KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07858", Fecha="31/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="VITOL INC",                         KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07837", Fecha="22/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",              KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07853", Fecha="30/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                       KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08274", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                       KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08275", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="ELEMENTA FOODS CHILE S.A.",         KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08293", Fecha="06/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",              KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                ]
                            },
                            new PeriodoGroup
                            {
                                Periodo = "JUL/2026",
                                Total   = "16,000,000",
                                Rows =
                                [
                                    new ContratoRow { Contrato="08274", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                             KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08275", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="ELEMENTA FOODS CHILE S.A.",               KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08290", Fecha="06/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="MOLINOS OVERSEAS COMMODITIES S.A.",      KilosCtr="6,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08293", Fecha="06/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",                   KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07837", Fecha="22/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",                   KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07853", Fecha="30/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                            KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08269", Fecha="03/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                            KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07858", Fecha="31/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="VITOL INC",                              KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                ]
                            }
                        ]
                    },
                    new SeccionContratos
                    {
                        Nombre = "Compras",
                        Total  = "14,000,000",
                        Periodos =
                        [
                            new PeriodoGroup
                            {
                                Periodo = "JUN/2026",
                                Total   = "7,000,000",
                                Rows =
                                [
                                    new ContratoRow { Contrato="07963", Fecha="12/12/2025", Vendedor="BUNGE S.A.",                  Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08164", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08165", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08171", Fecha="30/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08057", Fecha="09/01/2026", Vendedor="BUNGE S.A.",                  Comprador="COFCO RESOURCES S.A.", KilosCtr="3,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                ]
                            },
                            new PeriodoGroup
                            {
                                Periodo = "JUL/2026",
                                Total   = "7,000,000",
                                Rows =
                                [
                                    new ContratoRow { Contrato="08057", Fecha="09/01/2026", Vendedor="BUNGE S.A.",                  Comprador="COFCO RESOURCES S.A.", KilosCtr="3,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08164", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08165", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="08171", Fecha="30/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY", Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    new ContratoRow { Contrato="07963", Fecha="12/12/2025", Vendedor="BUNGE S.A.",                  Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                ]
                            }
                        ]
                    }
                ]
            },
        ]
    };
}
