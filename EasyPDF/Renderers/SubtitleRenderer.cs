using EasyPDF.Annotations.Content;
using EasyPDF.Interfaces;
using EasyPDF.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Renderers;

public sealed class SubtitleRenderer : IElementRenderer
{
    private readonly IContainerStyleApplier _containerStyleApplier;
    private readonly ITextStyleApplier _textStyleApplier;

    public SubtitleRenderer(IContainerStyleApplier containerStyleApplier, ITextStyleApplier textStyleApplier)
    {
        _containerStyleApplier = containerStyleApplier;
        _textStyleApplier = textStyleApplier;
    }

    public bool CanRender(PropertyDescriptor descriptor) =>
        descriptor.HasAttribute<PdfSubtitleAttribute>();

    public void Render(IContainer container, PropertyDescriptor descriptor)
    {
        var attrs = descriptor.AllAttributes;
        var styled = _containerStyleApplier.Apply(container, attrs);

        styled.Text(text =>
        {
            var span = text.Span(descriptor.Value?.ToString() ?? string.Empty).FontSize(18f);
            _textStyleApplier.Apply(span, attrs);
        });
    }
}
