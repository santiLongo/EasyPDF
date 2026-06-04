using EasyPDF.Builder.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder;

public sealed class SectionBuilder
{
    private readonly List<IBuilderElement> _elements = new();

    // ── Text ──────────────────────────────────────────────────────────────────

    public TextElement Text(string text)
    {
        var el = new TextElement(text);
        _elements.Add(el);
        return el;
    }

    // ── Separator ─────────────────────────────────────────────────────────────

    public SectionBuilder Separator(float thickness = 1f, string color = "#000000", float spacing = 4f)
    {
        _elements.Add(new SeparatorElement(thickness, color, spacing));
        return this;
    }

    // ── Spacing ───────────────────────────────────────────────────────────────

    public SectionBuilder Spacing(float points)
    {
        _elements.Add(new SpacingElement(points));
        return this;
    }

    // ── Page number ───────────────────────────────────────────────────────────

    public PageNumberElement PageNumber(string format = "Page {page} of {total}")
    {
        var el = new PageNumberElement(format);
        _elements.Add(el);
        return el;
    }

    // ── Row ───────────────────────────────────────────────────────────────────

    public RowElement Row(Action<RowElement> configure)
    {
        var el = new RowElement();
        configure(el);
        _elements.Add(el);
        return el;
    }

    // ── Table ─────────────────────────────────────────────────────────────────

    public TableElement<T> Table<T>(
        IEnumerable<T>            data,
        Action<TableElement<T>>   configure)
    {
        var el = new TableElement<T>(data);
        configure(el);
        _elements.Add(el);
        return el;
    }

    // ── Column header row (use in page header to mirror ReportTable columns) ─────

    public ColumnHeaderRowElement ColumnHeaderRow(Action<ColumnHeaderRowElement> configure)
    {
        var el = new ColumnHeaderRowElement();
        configure(el);
        _elements.Add(el);
        return el;
    }

    // ── Report table (data + span + subtotal rows via foreach) ─────────────────

    public ReportTableElement<T> ReportTable<T>(Action<ReportTableElement<T>> configure)
    {
        var el = new ReportTableElement<T>();
        configure(el);
        _elements.Add(el);
        return el;
    }

    // ── Render ────────────────────────────────────────────────────────────────

    internal void Render(IContainer container)
    {
        container.Column(col =>
        {
            foreach (var el in _elements)
                el.Render(col.Item());
        });
    }
}
