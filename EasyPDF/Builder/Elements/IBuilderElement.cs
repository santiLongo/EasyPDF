using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Elements;

public interface IBuilderElement
{
    void Render(IContainer container);
}
