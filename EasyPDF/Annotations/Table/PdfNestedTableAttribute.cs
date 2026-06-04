namespace EasyPDF.Annotations.Table;

/// <summary>
/// Marks a List&lt;T&gt; property on a table-row class to render as an expandable sub-table
/// spanning all columns beneath the parent row.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class PdfNestedTableAttribute : Attribute
{
    public string HeaderBackgroundColor { get; set; } = "#555555";
    public string HeaderFontColor      { get; set; } = "#FFFFFF";
    public bool   ShowHeader           { get; set; } = true;
}
