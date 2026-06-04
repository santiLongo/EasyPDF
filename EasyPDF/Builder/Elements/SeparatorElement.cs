using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Elements;

internal sealed class SeparatorElement : IBuilderElement
{
    private readonly float  _thickness;
    private readonly string _color;
    private readonly float  _spacing;

    internal SeparatorElement(float thickness = 1f, string color = "#000000", float spacing = 4f)
    {
        _thickness = thickness;
        _color     = color;
        _spacing   = spacing;
    }

    public void Render(IContainer container)
    {
        container
            .PaddingVertical(_spacing, Unit.Point)
            .LineHorizontal(_thickness, Unit.Point)
            .LineColor(_color);
    }
}
