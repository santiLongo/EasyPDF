using EasyPDF.Annotations.Styling;
using EasyPDF.Enums;
using EasyPDF.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Styling;

public sealed class ContainerStyleApplier : IContainerStyleApplier
{
    public IContainer Apply(IContainer container, IReadOnlyList<Attribute> attributes)
    {
        var result = container;

        var spacing = attributes.OfType<PdfSpacingAttribute>().FirstOrDefault();
        if (spacing is not null)
        {
            if (spacing.Before > 0) result = result.PaddingTop(spacing.Before, Unit.Point);
            if (spacing.After > 0) result = result.PaddingBottom(spacing.After, Unit.Point);
        }

        var padding = attributes.OfType<PdfPaddingAttribute>().FirstOrDefault();
        if (padding is not null)
        {
            result = result
                .PaddingTop(padding.Top, Unit.Point)
                .PaddingBottom(padding.Bottom, Unit.Point)
                .PaddingLeft(padding.Left, Unit.Point)
                .PaddingRight(padding.Right, Unit.Point);
        }

        var bg = attributes.OfType<PdfBackgroundColorAttribute>().FirstOrDefault();
        if (bg is not null) result = result.Background(bg.Color);

        var border = attributes.OfType<PdfBorderAttribute>().FirstOrDefault();
        if (border is not null) result = result.Border(border.Thickness, (Color)border.Color).BorderColor(border.Color);

        var alignment = attributes.OfType<PdfAlignmentAttribute>().FirstOrDefault();
        if (alignment is not null)
        {
            result = alignment.Alignment switch
            {
                PdfAlignment.Center => result.AlignCenter(),
                PdfAlignment.Right => result.AlignRight(),
                _ => result.AlignLeft()
            };
        }

        return result;
    }
}
