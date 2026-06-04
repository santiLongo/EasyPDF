using EasyPDF.Sample.Models.PosicionCliente;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EasyPDF.Sample.Documents;

public sealed class PosicionClienteDocument : IDocument
{
    private readonly PosicionClienteData _data;

    // Relative column widths (A4 landscape)
    private const float CContrato  = 1.4f;
    private const float CFecha     = 1.8f;
    private const float CVendedor  = 3.8f;
    private const float CComprador = 3.8f;
    private const float CKilos     = 2.2f;
    private const float CPrecio    = 1.3f;
    private const float CPuerto    = 1.8f;
    private const float CPeriodo   = 1.6f;
    private const float CProducto  = 2.6f;
    private const uint  TotalCols  = 9;

    private const string ColorHeaderBg    = "#000000";
    private const string ColorHeaderText  = "#FFFFFF";
    private const string ColorBorder      = "#000000";
    private const float  FontBase         = 7.5f;
    private const float  CellPad          = 2.5f;

    public PosicionClienteDocument(PosicionClienteData data) => _data = data;

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(new PageSize(297, 210, Unit.Millimetre));
            page.MarginHorizontal(1.2f, Unit.Centimetre);
            page.MarginTop(0.8f, Unit.Centimetre);
            page.MarginBottom(0.8f, Unit.Centimetre);
            page.DefaultTextStyle(x => x.FontSize(FontBase));

            page.Header().Element(ComposePageHeader);
            page.Content().Element(ComposeContent);
        });
    }

    // ── Page header (repeats every page) ──────────────────────────────────────

    private void ComposePageHeader(IContainer container)
    {
        container.Column(col =>
        {
            // Row 1: empresa | título centrado | fecha+hora
            col.Item().PaddingBottom(2, Unit.Point).Row(row =>
            {
                row.AutoItem()
                   .AlignBottom()
                   .Text(_data.Empresa)
                   .Bold()
                   .FontSize(10f);

                row.RelativeItem().AlignCenter().Column(titleCol =>
                {
                    titleCol.Item().AlignCenter()
                        .Text(_data.Titulo)
                        .Bold()
                        .FontSize(15f)
                        .Underline(true);

                    titleCol.Item().AlignCenter()
                        .Text(_data.Periodos)
                        .Bold()
                        .FontSize(9f);
                });

                row.AutoItem().AlignBottom().Column(dateCol =>
                {
                    dateCol.Item().AlignRight().Text(_data.Fecha).FontSize(7.5f);
                    dateCol.Item().AlignRight().Text(_data.Hora).FontSize(7.5f);
                });
            });

            // Row 2: filtros
            col.Item().PaddingBottom(3, Unit.Point).AlignCenter().Text(text =>
            {
                text.Span("Cliente :  ").Bold().FontSize(8.5f);
                text.Span(_data.ClienteFilter).Bold().FontSize(8.5f);
                text.Span("   Producto :  ").Bold().FontSize(8.5f);
                text.Span(_data.ProductoFilter).Bold().FontSize(8.5f);
            });

            // Row 3: column headers (table inside header so it aligns with content table)
            col.Item().Table(table =>
            {
                DefineColumns(table);
                RenderColumnHeaders(table);
            });
        });
    }

    // ── Content ────────────────────────────────────────────────────────────────

    private void ComposeContent(IContainer container)
    {
        container.Table(table =>
        {
            DefineColumns(table);

            foreach (var cliente in _data.Clientes)
                RenderCliente(table, cliente);
        });
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private static void DefineColumns(TableDescriptor table)
    {
        table.ColumnsDefinition(cols =>
        {
            cols.RelativeColumn(CContrato);
            cols.RelativeColumn(CFecha);
            cols.RelativeColumn(CVendedor);
            cols.RelativeColumn(CComprador);
            cols.RelativeColumn(CKilos);
            cols.RelativeColumn(CPrecio);
            cols.RelativeColumn(CPuerto);
            cols.RelativeColumn(CPeriodo);
            cols.RelativeColumn(CProducto);
        });
    }

    private static void RenderColumnHeaders(TableDescriptor table)
    {
        string[] headers = ["Contrato", "Fecha", "Vendedor", "Comprador", "Kilos Ctrat", "Precio", "Puerto", "Período", "Producto"];
        foreach (var h in headers)
        {
            table.Cell()
                .Background(ColorHeaderBg)
                .Border(0.4f, (Color)ColorBorder)
                .Padding(CellPad, Unit.Point)
                .Text(h)
                .Bold()
                .FontColor(ColorHeaderText)
                .FontSize(FontBase);
        }
    }

    private static void RenderCliente(TableDescriptor table, ClienteSection cliente)
    {
        // ── Client header row
        table.Cell().ColumnSpan(TotalCols)
            .PaddingVertical(3, Unit.Point)
            .Text(text =>
            {
                text.Span($"Cliente : {cliente.ClienteId}   ").Bold().FontSize(8.5f);
                text.Span(cliente.ClienteNombre).Bold().FontSize(8.5f);
            });

        foreach (var seccion in cliente.Secciones)
            RenderSeccion(table, seccion);

        // ── Client net total
        RenderSubtotalRow(table, "Total Cliente Compras vs Ventas", cliente.TotalNetoCv, isBold: true);
    }

    private static void RenderSeccion(TableDescriptor table, SeccionContratos seccion)
    {
        // ── Section header (Ventas / Compras)
        table.Cell().ColumnSpan(TotalCols)
            .PaddingTop(1, Unit.Point)
            .Text(seccion.Nombre)
            .Bold()
            .FontSize(8f);

        foreach (var periodo in seccion.Periodos)
        {
            foreach (var row in periodo.Rows)
                RenderDataRow(table, row);

            RenderSubtotalRow(table, $"Total Período {periodo.Periodo}", periodo.Total, isBold: false);
        }

        RenderSubtotalRow(table, $"Total {seccion.Nombre}", seccion.Total, isBold: false);
    }

    private static void RenderDataRow(TableDescriptor table, ContratoRow row)
    {
        Cell(table).Text(row.Contrato);
        Cell(table).Text(row.Fecha);
        Cell(table).Text(row.Vendedor);
        Cell(table).Text(row.Comprador).Bold();
        Cell(table).AlignRight().Text(row.KilosCtr);
        Cell(table).AlignRight().Text(row.Precio).Bold();
        Cell(table).Text(row.Puerto).Bold();
        Cell(table).Text(row.Periodo).Bold();
        Cell(table).Text(row.Producto).Bold();
    }

    private static void RenderSubtotalRow(TableDescriptor table, string label, string amount, bool isBold)
    {
        // Cols 1-3: empty span
        table.Cell().ColumnSpan(3).Text("");

        // Col 4: label right-aligned
        var labelContainer = Cell(table).AlignRight();
        if (isBold)
            labelContainer.Text(label).Bold().FontSize(FontBase);
        else
            labelContainer.Text(label).FontSize(FontBase);

        // Col 5: amount with top border
        var amountText = table.Cell()
            .BorderTop(0.5f, Unit.Point)
            .BorderColor(ColorBorder)
            .Padding(CellPad, Unit.Point)
            .AlignRight()
            .Text(amount)
            .Bold();

        if (isBold) amountText.FontSize(FontBase);

        // Cols 6-9: empty span
        table.Cell().ColumnSpan(4).Text("");
    }

    // Shortcut for a standard data cell
    private static IContainer Cell(TableDescriptor table) =>
        table.Cell().Padding(CellPad, Unit.Point);
}
