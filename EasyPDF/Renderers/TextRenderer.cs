using EasyPDF.Annotations.Content;
using EasyPDF.Interfaces;
using EasyPDF.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Renderers;

public sealed class TextRenderer : IElementRenderer
{
    private readonly IContainerStyleApplier _containerStyleApplier;
    private readonly ITextStyleApplier _textStyleApplier;

    public TextRenderer(IContainerStyleApplier containerStyleApplier, ITextStyleApplier textStyleApplier)
    {
        _containerStyleApplier = containerStyleApplier;
        _textStyleApplier = textStyleApplier;
    }

    public bool CanRender(PropertyDescriptor descriptor) =>
        descriptor.HasAttribute<PdfTextAttribute>();

    public void Render(IContainer container, PropertyDescriptor descriptor)
    {
        var textAttr = descriptor.GetAttribute<PdfTextAttribute>()!;
        var attrs = descriptor.AllAttributes;
        var styled = _containerStyleApplier.Apply(container, attrs);

        styled.Text(text =>
        {
            if (!string.IsNullOrEmpty(textAttr.Label))
                text.Span(textAttr.Label + " ").Bold();

            var span = text.Span(descriptor.Value?.ToString() ?? string.Empty);
            _textStyleApplier.Apply(span, attrs);
        });
    }
}
