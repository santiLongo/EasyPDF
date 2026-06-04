using EasyPDF.Models;

namespace EasyPDF.Interfaces;

public interface IRendererRegistry
{
    void Register(IElementRenderer renderer);
    IElementRenderer? Resolve(PropertyDescriptor descriptor);
    IEnumerable<IElementRenderer> GetAll();
}
