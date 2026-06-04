using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Unit = QuestPDF.Infrastructure.Unit;

namespace EasyPDF.Builder.Elements;

// ── Internal row contract ────────────────────────────────────────────────────

internal interface IReportRow
{
    void Render(TableDescriptor table, int totalCols);
}

// ── Row: typed data row ───────────────────────────────────────────────────────

internal sealed class DataReportRow<T> : IReportRow
{
    private readonly T                              _item;
    private readonly IReadOnlyList<ReportColumn<T>> _columns;

    internal DataReportRow(T item, IReadOnlyList<ReportColumn<T>> columns)
    {
        _item    = item;
        _columns = columns;
    }

    public void Render(TableDescriptor table, int _)
    {
        foreach (var col in _columns)
            col.RenderCell(table.Cell(), _item);
    }
}

// ── Row: full-width span (client header, section title, …) ───────────────────

internal sealed class SpanReportRow : IReportRow
{
    private readonly string _text;
    private readonly bool   _bold;
    private readonly float  _fontSize;
    private readonly float  _paddingTop;
    private readonly float  _paddingBottom;

    internal SpanReportRow(string text, bool bold, float fontSize,
                           float paddingTop, float paddingBottom)
    {
        _text          = text;
        _bold          = bold;
        _fontSize      = fontSize;
        _paddingTop    = paddingTop;
        _paddingBottom = paddingBottom;
    }

    public void Render(TableDescriptor table, int totalCols)
    {
        var cell = table.Cell()
            .ColumnSpan((uint)totalCols)
            .PaddingTop(_paddingTop, Unit.Point)
            .PaddingBottom(_paddingBottom, Unit.Point);

        var t = cell.Text(_text);
        if (_bold) t.Bold();
        t.FontSize(_fontSize);
    }
}

// ── Row: subtotal (empty × n | label right | amount border-top | empty × m) ──

internal sealed class SubtotalReportRow : IReportRow
{
    private readonly string _label;
    private readonly string _amount;
    private readonly bool   _isBold;
    private readonly int    _emptyBefore;
    private readonly float  _fontSize;
    private readonly float  _padding;

    internal SubtotalReportRow(string label, string amount, bool isBold,
                               int emptyBefore, float fontSize, float padding)
    {
        _label       = label;
        _amount      = amount;
        _isBold      = isBold;
        _emptyBefore = emptyBefore;
        _fontSize    = fontSize;
        _padding     = padding;
    }

    public void Render(TableDescriptor table, int totalCols)
    {
        // ── Empty cells before label ───────────────────────────────────────
        if (_emptyBefore == 1)
            table.Cell().Text("");
        else if (_emptyBefore > 1)
            table.Cell().ColumnSpan((uint)_emptyBefore).Text("");

        // ── Label (right-aligned) ──────────────────────────────────────────
        var labelCell = table.Cell().Padding(_padding, Unit.Point).AlignRight();
        var labelText = labelCell.Text(_label).FontSize(_fontSize);
        if (_isBold) labelText.Bold();

        // ── Amount (top border, right-aligned, bold) ───────────────────────
        var amountText = table.Cell()
            .BorderTop(0.5f, Unit.Point)
            .BorderColor("#000000")
            .Padding(_padding, Unit.Point)
            .AlignRight()
            .Text(_amount)
            .Bold()
            .FontSize(_fontSize);

        // ── Trailing empty cells ───────────────────────────────────────────
        int emptyAfter = totalCols - _emptyBefore - 2;
        if (emptyAfter == 1)
            table.Cell().Text("");
        else if (emptyAfter > 1)
            table.Cell().ColumnSpan((uint)emptyAfter).Text("");
    }
}

// ── Column definition (internal to ReportTableElement) ───────────────────────

public sealed class ReportColumn<T>
{
    internal readonly Func<T, string> Selector;
    internal readonly float           Width;
    private bool    _bold;
    private bool    _alignRight;
    private bool    _alignCenter;
    private string? _color;
    private float?  _fontSize;
    private float   _padding = 2.5f;

    internal ReportColumn(Func<T, string> selector, float width)
    {
        Selector = selector;
        Width    = width;
    }

    public ReportColumn<T> Bold()            { _bold        = true; return this; }
    public ReportColumn<T> AlignRight()      { _alignRight  = true; return this; }
    public ReportColumn<T> AlignCenter()     { _alignCenter = true; return this; }
    public ReportColumn<T> Color(string hex) { _color       = hex;  return this; }
    public ReportColumn<T> FontSize(float s) { _fontSize    = s;    return this; }
    public ReportColumn<T> Padding(float pt) { _padding     = pt;   return this; }

    internal void RenderCell(IContainer cell, T item)
    {
        var padded  = cell.Padding(_padding, Unit.Point);
        var aligned = _alignCenter ? padded.AlignCenter()
                    : _alignRight  ? padded.AlignRight()
                    : padded.AlignLeft();

        aligned.Text(text =>
        {
            var span = text.Span(Selector(item));
            if (_bold)              span = span.Bold();
            if (_fontSize.HasValue) span = span.FontSize(_fontSize.Value);
            if (_color is not null) span = span.FontColor(_color);
        });
    }
}

// ── Main element ─────────────────────────────────────────────────────────────

/// <summary>
/// A single QuestPDF table that supports interleaved data rows, full-width
/// span rows, and subtotal rows — all driven by foreach loops.
/// </summary>
public sealed class ReportTableElement<T> : IBuilderElement
{
    private readonly List<ReportColumn<T>> _columns    = new();
    private readonly List<IReportRow>      _rows       = new();
    private bool   _showHeader   = false;
    private string _headerBg     = "#000000";
    private string _headerFg     = "#FFFFFF";
    private float  _defaultFontSize = 7.5f;
    private float  _defaultPadding  = 2.5f;

    // ── Column definition ─────────────────────────────────────────────────────

    public ReportColumn<T> Column(
        Func<T, string>          selector,
        float                    width,
        Action<ReportColumn<T>>? configure = null)
    {
        var col = new ReportColumn<T>(selector, width);
        configure?.Invoke(col);
        _columns.Add(col);
        return col;
    }

    // ── Table-level options ────────────────────────────────────────────────────

    /// <summary>Show a built-in table header (uses table.Header() — repeats on page breaks).</summary>
    public ReportTableElement<T> ShowHeader(bool show = true) { _showHeader = show; return this; }
    public ReportTableElement<T> HeaderBackground(string hex) { _headerBg   = hex;  return this; }
    public ReportTableElement<T> HeaderForeground(string hex) { _headerFg   = hex;  return this; }

    // ── Row builders (call inside foreach loops) ──────────────────────────────

    /// <summary>Add a typed data row.</summary>
    public ReportTableElement<T> DataRow(T item)
    {
        _rows.Add(new DataReportRow<T>(item, _columns.AsReadOnly()));
        return this;
    }

    /// <summary>Full-width spanning text row (e.g. client name, section title).</summary>
    public ReportTableElement<T> SpanRow(
        string text,
        bool   bold          = false,
        float  fontSize      = 0f,
        float  paddingTop    = 0f,
        float  paddingBottom = 0f)
    {
        _rows.Add(new SpanReportRow(
            text,
            bold,
            fontSize > 0 ? fontSize : _defaultFontSize,
            paddingTop,
            paddingBottom));
        return this;
    }

    /// <summary>
    /// Subtotal row: [emptyBefore empty cells] [label right] [amount border-top] [trailing empty cells].
    /// emptyBefore defaults to 3 (matching 9-column layout col 4 = label, col 5 = amount).
    /// </summary>
    public ReportTableElement<T> SubtotalRow(
        string label,
        string amount,
        bool   isBold      = false,
        int    emptyBefore = 3,
        float  fontSize    = 0f)
    {
        _rows.Add(new SubtotalReportRow(
            label,
            amount,
            isBold,
            emptyBefore,
            fontSize > 0 ? fontSize : _defaultFontSize,
            _defaultPadding));
        return this;
    }

    // ── Render ────────────────────────────────────────────────────────────────

    public void Render(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(def =>
            {
                foreach (var c in _columns)
                    def.RelativeColumn(c.Width);
            });

            if (_showHeader)
            {
                table.Header(header =>
                {
                    foreach (var c in _columns)
                    {
                        header.Cell()
                            .Background(_headerBg)
                            .Padding(_defaultPadding, Unit.Point)
                            .Text("") // headers come from ColumnHeaderRowElement
                            .Bold()
                            .FontColor(_headerFg)
                            .FontSize(_defaultFontSize);
                    }
                });
            }

            foreach (var row in _rows)
                row.Render(table, _columns.Count);
        });
    }
}
