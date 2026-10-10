using EasyPDF.Enums;
using EasyPDF.Interfaces;
using EasyPDF.Models;
using EasyPDF.Renderers;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using EasyPDF.ExtensionMethods;

namespace EasyPDF.Services;

public sealed class PdfGeneratorService : IPdfGenerator
{
    private readonly IAnnotationResolver _annotationResolver;
    private readonly IRendererRegistry   _rendererRegistry;

    public PdfGeneratorService(IAnnotationResolver annotationResolver, IRendererRegistry rendererRegistry)
    {
        _annotationResolver = annotationResolver;
        _rendererRegistry   = rendererRegistry;
    }

    public byte[] Generate(IPdfDocument model) =>
        BuildDocument(model).GeneratePdf();

    public void GenerateToFile(IPdfDocument model, string fileName) =>
        BuildDocument(model).GeneratePdf(fileName);
    
    public void GenerateToFile(IPdfDocument model, string fileName, string  path) =>
        BuildDocument(model).GeneratePdf(fileName, path);

    public Stream GenerateToStream(IPdfDocument model)
    {
        var bytes = Generate(model);
        return new MemoryStream(bytes);
    }

    private IDocument BuildDocument(IPdfDocument model)
    {
        var pageConfig = _annotationResolver.ResolvePageConfiguration(model.GetType());
        var properties = _annotationResolver.Resolve(model);
        var header     = _annotationResolver.ResolveHeader(model);
        var footer     = _annotationResolver.ResolveFooter(model);
        var hfRenderer = new HeaderFooterRenderer(_rendererRegistry);

        return Document.Create(container =>
            ComposeDocument(container, properties, pageConfig, header, footer, hfRenderer));
    }

    private void ComposeDocument(
        IDocumentContainer           container,
        IReadOnlyList<PropertyDescriptor> properties,
        PageConfiguration            pageConfig,
        PropertyDescriptor?          header,
        PropertyDescriptor?          footer,
        HeaderFooterRenderer         hfRenderer)
    {
        container.Page(page =>
        {
            page.Size(MapPageSize(pageConfig.PageSize));
            page.Margin(pageConfig.MarginValue, MapUnit(pageConfig.MarginUnit));
            page.PageColor(pageConfig.PageColor);
            page.DefaultTextStyle(x => x.FontSize(pageConfig.DefaultFontSize));

            if (header is not null)
                page.Header().Element(c => hfRenderer.Render(c, header));

            if (footer is not null)
                page.Footer().Element(c => hfRenderer.Render(c, footer));

            page.Content().Column(column =>
            {
                foreach (var descriptor in properties)
                {
                    var renderer = _rendererRegistry.Resolve(descriptor);
                    renderer?.Render(column.Item(), descriptor);
                }
            });
        });
    }

    private static PageSize MapPageSize(PdfPageSizeType size) => size switch
    {
        PdfPageSizeType.A3      => PageSizes.A3,
        PdfPageSizeType.A5      => PageSizes.A5,
        PdfPageSizeType.Letter  => PageSizes.Letter,
        PdfPageSizeType.Legal   => PageSizes.Legal,
        PdfPageSizeType.Tabloid => PageSizes.Tabloid,
        _                       => PageSizes.A4
    };

    private static Unit MapUnit(PdfUnit unit) => unit switch
    {
        PdfUnit.Point      => Unit.Point,
        PdfUnit.Millimetre => Unit.Millimetre,
        PdfUnit.Inch       => Unit.Inch,
        _                  => Unit.Centimetre
    };
}
