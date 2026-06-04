using EasyPDF.Annotations.Content;
using EasyPDF.Interfaces;
using EasyPDF.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Renderers;

public sealed class ImageRenderer : IElementRenderer
{
    private readonly IContainerStyleApplier _containerStyleApplier;

    public ImageRenderer(IContainerStyleApplier containerStyleApplier)
    {
        _containerStyleApplier = containerStyleApplier;
    }

    public bool CanRender(PropertyDescriptor descriptor) =>
        descriptor.HasAttribute<PdfImageAttribute>();

    public void Render(IContainer container, PropertyDescriptor descriptor)
    {
        var imageAttr = descriptor.GetAttribute<PdfImageAttribute>()!;
        var attrs = descriptor.AllAttributes;

        var styled = _containerStyleApplier.Apply(container, attrs);
        styled = ApplyDimensions(styled, imageAttr);

        switch (descriptor.Value)
        {
            case byte[] imageBytes:
                styled.Image(imageBytes).FitWidth();
                break;
            case string imagePath when !string.IsNullOrWhiteSpace(imagePath):
                styled.Image(imagePath).FitWidth();
                break;
        }
    }

    private static IContainer ApplyDimensions(IContainer container, PdfImageAttribute attr)
    {
        var result = container;
        if (attr.Width.HasValue) result = result.Width(attr.Width.Value, Unit.Point);
        if (attr.Height.HasValue) result = result.Height(attr.Height.Value, Unit.Point);
        return result;
    }
}
