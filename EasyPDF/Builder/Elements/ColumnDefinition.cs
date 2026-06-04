using System.Linq.Expressions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Elements;

public sealed class ColumnDefinition<T>
{
    internal readonly Func<T, string> ValueSelector;
    internal readonly string          Header;
    internal readonly float           Width;
    private bool   _bold;
    private bool   _alignRight;
    private bool   _alignCenter;
    private string? _color;
    private float?  _fontSize;

    internal ColumnDefinition(Func<T, string> selector, string header, float width)
    {
        ValueSelector = selector;
        Header        = header;
        Width         = width;
    }

    public ColumnDefinition<T> Bold()             { _bold        = true;  return this; }
    public ColumnDefinition<T> AlignRight()       { _alignRight  = true;  return this; }
    public ColumnDefinition<T> AlignCenter()      { _alignCenter = true;  return this; }
    public ColumnDefinition<T> Color(string hex)  { _color       = hex;   return this; }
    public ColumnDefinition<T> FontSize(float s)  { _fontSize    = s;     return this; }

    internal void RenderCell(IContainer cell, T item, string? bg = null)
    {
        var padded = cell.Padding(5, Unit.Point);
        if (bg is not null) padded = padded.Background(bg);

        var aligned = _alignCenter ? padded.AlignCenter()
                    : _alignRight  ? padded.AlignRight()
                    : padded.AlignLeft();

        aligned.Text(text =>
        {
            var span = text.Span(ValueSelector(item));
            if (_bold)             span = span.Bold();
            if (_fontSize.HasValue) span = span.FontSize(_fontSize.Value);
            if (_color is not null) span = span.FontColor(_color);
        });
    }
}
