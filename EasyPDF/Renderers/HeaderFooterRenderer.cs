using System.Reflection;
using EasyPDF.Annotations.Content;
using EasyPDF.Interfaces;
using EasyPDF.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Renderers;

/// <summary>
/// Renders the value of a [PdfPageHeaderContent] or [PdfPageFooterContent] property
/// by treating that value as an annotated sub-object and rendering each of its
/// PdfElement-decorated properties in order into the provided container.
/// </summary>
public sealed class HeaderFooterRenderer
{
    private readonly IRendererRegistry _registry;

    public HeaderFooterRenderer(IRendererRegistry registry)
    {
        _registry = registry;
    }

    public void Render(IContainer container, PropertyDescriptor zone)
    {
        var obj = zone.Value;
        if (obj is null) return;

        var descriptors = obj.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.IsDefined(typeof(PdfElementAttribute), false))
            .Where(p => !p.IsDefined(typeof(PdfIgnoreAttribute), false))
            .Select(p => new PropertyDescriptor(
                p,
                p.GetValue(obj),
                p.GetCustomAttributes().Cast<Attribute>()
            ))
            .OrderBy(d => d.GetAttribute<PdfElementAttribute>()!.Order)
            .ToList();

        container.Column(col =>
        {
            foreach (var descriptor in descriptors)
            {
                var renderer = _registry.Resolve(descriptor);
                renderer?.Render(col.Item(), descriptor);
            }
        });
    }
}
