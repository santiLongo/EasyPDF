using EasyPDF.Interfaces;
using EasyPDF.Registry;
using EasyPDF.Renderers;
using EasyPDF.Resolvers;
using EasyPDF.Services;
using EasyPDF.Styling;
using Microsoft.Extensions.DependencyInjection;

namespace EasyPDF.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEasyPdf(this IServiceCollection services)
    {
        services.AddSingleton<IContainerStyleApplier, ContainerStyleApplier>();
        services.AddSingleton<ITextStyleApplier, TextStyleApplier>();
        services.AddSingleton<ITableColumnResolver, TableColumnResolver>();
        services.AddSingleton<IAnnotationResolver, PropertyAnnotationResolver>();

        services.AddSingleton<IElementRenderer, TitleRenderer>();
        services.AddSingleton<IElementRenderer, SubtitleRenderer>();
        services.AddSingleton<IElementRenderer, TextRenderer>();
        services.AddSingleton<IElementRenderer, TableRenderer>();
        services.AddSingleton<IElementRenderer, ImageRenderer>();
        services.AddSingleton<IElementRenderer, SeparatorRenderer>();

        services.AddSingleton<IRendererRegistry>(sp =>
        {
            var registry = new RendererRegistry();
            foreach (var renderer in sp.GetServices<IElementRenderer>())
                registry.Register(renderer);
            return registry;
        });

        services.AddSingleton<IPdfGenerator, PdfGeneratorService>();

        return services;
    }
}
