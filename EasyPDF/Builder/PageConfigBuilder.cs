using EasyPDF.Enums;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder;

public sealed class PageConfigBuilder
{
    internal PageSize  Size            { get; private set; } = PageSizes.A4;
    internal float     MarginValue     { get; private set; } = 2f;
    internal Unit      MarginUnit      { get; private set; } = Unit.Centimetre;
    internal string    PageColor       { get; private set; } = "#FFFFFF";
    internal float     DefaultFontSizeValue { get; private set; } = 12f;

    // ── Page sizes ────────────────────────────────────────────────────────────

    public PageConfigBuilder A4()          { Size = PageSizes.A4;      return this; }
    public PageConfigBuilder A3()          { Size = PageSizes.A3;      return this; }
    public PageConfigBuilder A5()          { Size = PageSizes.A5;      return this; }
    public PageConfigBuilder Letter()      { Size = PageSizes.Letter;  return this; }
    public PageConfigBuilder Legal()       { Size = PageSizes.Legal;   return this; }

    public PageConfigBuilder Landscape()   { Size = Size.Landscape();  return this; }
    public PageConfigBuilder Portrait()    { Size = Size.Portrait();   return this; }

    public PageConfigBuilder Custom(float widthMm, float heightMm)
    {
        Size = new PageSize(widthMm, heightMm, Unit.Millimetre);
        return this;
    }

    // ── Margin ────────────────────────────────────────────────────────────────

    public PageConfigBuilder Margin(float value, PdfUnit unit = PdfUnit.Centimetre)
    {
        MarginValue = value;
        MarginUnit  = unit switch
        {
            PdfUnit.Point      => Unit.Point,
            PdfUnit.Millimetre => Unit.Millimetre,
            PdfUnit.Inch       => Unit.Inch,
            _                  => Unit.Centimetre
        };
        return this;
    }

    // ── Other ─────────────────────────────────────────────────────────────────

    public PageConfigBuilder BackgroundColor(string hex) { PageColor       = hex;  return this; }
    public PageConfigBuilder DefaultFontSize(float size) { DefaultFontSizeValue = size; return this; }
}
