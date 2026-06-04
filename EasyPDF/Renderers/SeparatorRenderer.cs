using EasyPDF.Annotations.Content;
using EasyPDF.Interfaces;
using EasyPDF.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Renderers;

public sealed class SeparatorRenderer : IElementRenderer
{
    public bool CanRender(PropertyDescriptor descriptor) =>
        descriptor.HasAttribute<PdfSeparatorAttribute>();

    public void Render(IContainer container, PropertyDescriptor descriptor)
    {
        var sep = descriptor.GetAttribute<PdfSeparatorAttribute>()!;

        container
            .PaddingVertical(sep.VerticalSpacing, Unit.Point)
            .LineHorizontal(sep.Thickness, Unit.Point)
            .LineColor(sep.Color);
    }
}
