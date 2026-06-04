using EasyPDF.Interfaces;
using EasyPDF.Models;

namespace EasyPDF.Registry;

public sealed class RendererRegistry : IRendererRegistry
{
    private readonly List<IElementRenderer> _renderers = new();

    public void Register(IElementRenderer renderer) =>
        _renderers.Add(renderer);

    public IElementRenderer? Resolve(PropertyDescriptor descriptor) =>
        _renderers.FirstOrDefault(r => r.CanRender(descriptor));

    public IEnumerable<IElementRenderer> GetAll() =>
        _renderers.AsReadOnly();
}
