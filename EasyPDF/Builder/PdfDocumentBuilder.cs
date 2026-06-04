using EasyPDF.Builder.Internal;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder;

/// <summary>
/// Fluent document designer — compose PDFs programmatically without annotations.
/// </summary>
/// <example>
/// <code>
/// var doc = PdfDocumentBuilder.Create()
///     .Page(p => p.A4().Landscape().Margin(1.2f).DefaultFontSize(8f))
///     .Header(h => {
///         h.Row(r => {
///             r.Auto().Add(h.Text("COMPANY").Bold().FontSize(10));
///             r.Fill().Add(h.Text("Report Title").FontSize(16).Bold().AlignCenter());
///         });
///     })
///     .Footer(f => f.PageNumber("Page {page} of {total}").AlignRight())
///     .Content(c => {
///         c.Table(data, t => {
///             t.Column(r => r.Name,  "Name",  width: 3f);
///             t.Column(r => r.Value, "Value", width: 1f).Bold().AlignRight();
///         });
///     })
///     .Build();
/// doc.GeneratePdf("output.pdf");
/// </code>
/// </example>
public sealed class PdfDocumentBuilder
{
    private PageConfigBuilder _pageConfig = new();
    private SectionBuilder?   _header;
    private SectionBuilder?   _footer;
    private SectionBuilder    _content = new();

    private PdfDocumentBuilder() { }

    public static PdfDocumentBuilder Create() => new();

    // ── Page ──────────────────────────────────────────────────────────────────

    public PdfDocumentBuilder Page(Action<PageConfigBuilder> configure)
    {
        configure(_pageConfig);
        return this;
    }

    // ── Header ────────────────────────────────────────────────────────────────

    public PdfDocumentBuilder Header(Action<SectionBuilder> configure)
    {
        _header = new SectionBuilder();
        configure(_header);
        return this;
    }

    // ── Footer ────────────────────────────────────────────────────────────────

    public PdfDocumentBuilder Footer(Action<SectionBuilder> configure)
    {
        _footer = new SectionBuilder();
        configure(_footer);
        return this;
    }

    // ── Content ───────────────────────────────────────────────────────────────

    public PdfDocumentBuilder Content(Action<SectionBuilder> configure)
    {
        configure(_content);
        return this;
    }

    // ── Output ───────────────────────────────────────────────────────────────

    /// <summary>Returns the underlying QuestPDF IDocument for full control.</summary>
    public IDocument Build() =>
        new BuilderDocument(_pageConfig, _header, _footer, _content);

    /// <summary>Writes the PDF to <paramref name="filePath"/>.</summary>
    public void GeneratePdf(string filePath) => Build().GeneratePdf(filePath);

    /// <summary>Returns the PDF as a byte array.</summary>
    public byte[] GeneratePdfBytes() => Build().GeneratePdf();

    /// <summary>Returns the PDF as a MemoryStream.</summary>
    public Stream GeneratePdfStream()
    {
        var bytes = GeneratePdfBytes();
        return new MemoryStream(bytes);
    }
}
