namespace EasyPDF.Annotations.Styling;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class PdfSpacingAttribute : Attribute
{
    public float Before { get; }
    public float After { get; }

    public PdfSpacingAttribute(float before = 0f, float after = 0f)
    {
        Before = before;
        After = after;
    }
}
