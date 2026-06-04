using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Elements;

public sealed class TextElement : IBuilderElement
{
    private readonly string _text;
    private bool   _bold;
    private bool   _italic;
    private bool   _underline;
    private float? _fontSize;
    private string? _color;
    private HorizontalAlignment _align = HorizontalAlignment.Left;

    internal TextElement(string text) => _text = text;

    public TextElement Bold()            { _bold      = true;  return this; }
    public TextElement Italic()          { _italic    = true;  return this; }
    public TextElement Underline()       { _underline = true;  return this; }
    public TextElement FontSize(float s) { _fontSize  = s;     return this; }
    public TextElement Color(string hex) { _color     = hex;   return this; }
    public TextElement AlignLeft()       { _align = HorizontalAlignment.Left;   return this; }
    public TextElement AlignCenter()     { _align = HorizontalAlignment.Center; return this; }
    public TextElement AlignRight()      { _align = HorizontalAlignment.Right;  return this; }

    public void Render(IContainer container)
    {
        var aligned = _align switch
        {
            HorizontalAlignment.Center => container.AlignCenter(),
            HorizontalAlignment.Right  => container.AlignRight(),
            _                          => container.AlignLeft()
        };

        aligned.Text(text =>
        {
            var span = text.Span(_text);
            if (_bold)      span = span.Bold();
            if (_italic)    span = span.Italic(true);
            if (_underline) span = span.Underline(true);
            if (_fontSize.HasValue) span = span.FontSize(_fontSize.Value);
            if (_color is not null) span = span.FontColor(_color);
        });
    }

    private enum HorizontalAlignment { Left, Center, Right }
}
