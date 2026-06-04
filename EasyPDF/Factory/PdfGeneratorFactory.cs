using EasyPDF.Interfaces;
using EasyPDF.Registry;
using EasyPDF.Renderers;
using EasyPDF.Resolvers;
using EasyPDF.Services;
using EasyPDF.Styling;

namespace EasyPDF.Factory;

public static class PdfGeneratorFactory
{
    public static IPdfGenerator Create()
    {
        var containerStyleApplier = new ContainerStyleApplier();
        var textStyleApplier = new TextStyleApplier();
        var columnResolver = new TableColumnResolver();

        var registry = new RendererRegistry();
        registry.Register(new TitleRenderer(containerStyleApplier, textStyleApplier));
        registry.Register(new SubtitleRenderer(containerStyleApplier, textStyleApplier));
        registry.Register(new TextRenderer(containerStyleApplier, textStyleApplier));
        registry.Register(new TableRenderer(containerStyleApplier, columnResolver));
        registry.Register(new ImageRenderer(containerStyleApplier));
        registry.Register(new SeparatorRenderer());

        var resolver = new PropertyAnnotationResolver();

        return new PdfGeneratorService(resolver, registry);
    }
}
