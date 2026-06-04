using EasyPDF.Annotations.Styling;
using EasyPDF.Interfaces;
using QuestPDF.Fluent;

namespace EasyPDF.Styling;

public sealed class TextStyleApplier : ITextStyleApplier
{
    public TextSpanDescriptor Apply(TextSpanDescriptor span, IReadOnlyList<Attribute> attributes)
    {
        var result = span;

        if (attributes.OfType<PdfBoldAttribute>().Any())
            result = result.Bold();

        if (attributes.OfType<PdfItalicAttribute>().Any())
            result = result.Italic(true);

        if (attributes.OfType<PdfUnderlineAttribute>().Any())
            result = result.Underline(true);

        var fontSize = attributes.OfType<PdfFontSizeAttribute>().FirstOrDefault();
        if (fontSize is not null) result = result.FontSize(fontSize.Size);

        var fontColor = attributes.OfType<PdfFontColorAttribute>().FirstOrDefault();
        if (fontColor is not null) result = result.FontColor(fontColor.Color);

        return result;
    }
}
