using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Internal;

internal sealed class BuilderDocument : IDocument
{
    private readonly PageConfigBuilder  _pageConfig;
    private readonly SectionBuilder?    _header;
    private readonly SectionBuilder?    _footer;
    private readonly SectionBuilder     _content;

    internal BuilderDocument(
        PageConfigBuilder pageConfig,
        SectionBuilder?   header,
        SectionBuilder?   footer,
        SectionBuilder    content)
    {
        _pageConfig = pageConfig;
        _header     = header;
        _footer     = footer;
        _content    = content;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(_pageConfig.Size);
            page.Margin(_pageConfig.MarginValue, _pageConfig.MarginUnit);
            page.PageColor(_pageConfig.PageColor);
            page.DefaultTextStyle(x => x.FontSize(_pageConfig.DefaultFontSizeValue));

            if (_header is not null)
                page.Header().Element(c => _header.Render(c));

            if (_footer is not null)
                page.Footer().Element(c => _footer.Render(c));

            page.Content().Element(c => _content.Render(c));
        });
    }
}
