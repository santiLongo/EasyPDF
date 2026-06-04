using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Elements;

/// <summary>
/// Renders a single header row as a table — use in page.Header() so it
/// repeats on every page and aligns with the ReportTableElement below.
/// </summary>
public sealed class ColumnHeaderRowElement : IBuilderElement
{
    private readonly List<(string Label, float Width)> _columns = new();
    private string _bg          = "#000000";
    private string _fg          = "#FFFFFF";
    private float  _fontSize    = 7.5f;
    private float  _padding     = 2.5f;
    private float  _border      = 0.4f;
    private string _borderColor = "#000000";

    public ColumnHeaderRowElement Column(string label, float width)
    {
        _columns.Add((label, width));
        return this;
    }

    public ColumnHeaderRowElement Background(string hex)  { _bg = hex;      return this; }
    public ColumnHeaderRowElement Foreground(string hex)  { _fg = hex;      return this; }
    public ColumnHeaderRowElement FontSize(float size)    { _fontSize = size; return this; }
    public ColumnHeaderRowElement Padding(float pt)       { _padding  = pt;  return this; }
    public ColumnHeaderRowElement Border(float pt, string color = "#000000")
    {
        _border      = pt;
        _borderColor = color;
        return this;
    }

    public void Render(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(def =>
            {
                foreach (var (_, w) in _columns)
                    def.RelativeColumn(w);
            });

            foreach (var (label, _) in _columns)
            {
                table.Cell()
                    .Background(_bg)
                    .Border(_border, Unit.Point)
                    .BorderColor(_borderColor)
                    .Padding(_padding, Unit.Point)
                    .Text(label)
                    .Bold()
                    .FontColor(_fg)
                    .FontSize(_fontSize);
            }
        });
    }
}
