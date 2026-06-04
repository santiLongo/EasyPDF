using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Elements;

internal sealed class SpacingElement : IBuilderElement
{
    private readonly float _points;
    internal SpacingElement(float points) => _points = points;

    public void Render(IContainer container) =>
        container.Height(_points, Unit.Point);
}
