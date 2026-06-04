using EasyPDF.Models;
using QuestPDF.Infrastructure;

namespace EasyPDF.Interfaces;

public interface IElementRenderer
{
    bool CanRender(PropertyDescriptor descriptor);
    void Render(IContainer container, PropertyDescriptor descriptor);
}
