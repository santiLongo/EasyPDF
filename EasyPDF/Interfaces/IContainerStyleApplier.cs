using QuestPDF.Infrastructure;

namespace EasyPDF.Interfaces;

public interface IContainerStyleApplier
{
    IContainer Apply(IContainer container, IReadOnlyList<Attribute> attributes);
}
