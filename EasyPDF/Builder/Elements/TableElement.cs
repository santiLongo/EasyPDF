using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Elements;

public sealed class TableElement<T> : IBuilderElement
{
    private readonly IReadOnlyList<T>               _data;
    private readonly List<ColumnDefinition<T>>      _columns = new();
    private string  _headerBg   = "#333333";
    private string  _headerFg   = "#FFFFFF";
    private bool    _striped    = false;
    private string  _stripedBg  = "#F5F5F5";
    private string? _title;

    internal TableElement(IEnumerable<T> data) =>
        _data = data.ToList().AsReadOnly();

    public TableElement<T> Column(
        Func<T, string>     selector,
        string              header,
        float               width          = 1f,
        Action<ColumnDefinition<T>>? configure = null)
    {
        var col = new ColumnDefinition<T>(selector, header, width);
        configure?.Invoke(col);
        _columns.Add(col);
        return this;
    }

    public TableElement<T> HeaderBackground(string hex)  { _headerBg  = hex;   return this; }
    public TableElement<T> HeaderForeground(string hex)  { _headerFg  = hex;   return this; }
    public TableElement<T> Striped(bool enable = true, string? color = null)
    {
        _striped   = enable;
        if (color is not null) _stripedBg = color;
        return this;
    }
    public TableElement<T> Title(string title) { _title = title; return this; }

    public void Render(IContainer container)
    {
        container.Column(col =>
        {
            if (_title is not null)
                col.Item().PaddingBottom(4, Unit.Point).Text(_title).Bold().FontSize(13f);

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(def =>
                {
                    foreach (var c in _columns)
                        def.RelativeColumn(c.Width);
                });

                if (_columns.Count > 0)
                {
                    table.Header(header =>
                    {
                        foreach (var c in _columns)
                        {
                            header.Cell()
                                .Background(_headerBg)
                                .Padding(5, Unit.Point)
                                .Text(c.Header)
                                .Bold()
                                .FontColor(_headerFg);
                        }
                    });
                }

                var items = _data.ToList();
                for (var i = 0; i < items.Count; i++)
                {
                    var bg = _striped && i % 2 == 1 ? _stripedBg : "#FFFFFF";
                    foreach (var c in _columns)
                        c.RenderCell(table.Cell(), items[i], bg);
                }
            });
        });
    }
}
