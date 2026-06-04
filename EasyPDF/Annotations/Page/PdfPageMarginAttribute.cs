using EasyPDF.Enums;

namespace EasyPDF.Annotations.Page;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PdfPageMarginAttribute : Attribute
{
    public float Value { get; }
    public PdfUnit Unit { get; }

    public PdfPageMarginAttribute(float value = 2f, PdfUnit unit = PdfUnit.Centimetre)
    {
        Value = value;
        Unit = unit;
    }
}
