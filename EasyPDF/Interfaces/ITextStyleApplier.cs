using QuestPDF.Fluent;

namespace EasyPDF.Interfaces;

public interface ITextStyleApplier
{
    TextSpanDescriptor Apply(TextSpanDescriptor span, IReadOnlyList<Attribute> attributes);
}
