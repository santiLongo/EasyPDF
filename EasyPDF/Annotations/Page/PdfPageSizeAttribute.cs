using EasyPDF.Enums;

namespace EasyPDF.Annotations.Page;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PdfPageSizeAttribute : Attribute
{
    public PdfPageSizeType Size { get; }

    public PdfPageSizeAttribute(PdfPageSizeType size = PdfPageSizeType.A4) => Size = size;
}
