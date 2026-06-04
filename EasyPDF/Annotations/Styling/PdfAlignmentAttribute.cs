using EasyPDF.Enums;

namespace EasyPDF.Annotations.Styling;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfAlignmentAttribute : Attribute
{
    public PdfAlignment Alignment { get; }

    public PdfAlignmentAttribute(PdfAlignment alignment) => Alignment = alignment;
}
