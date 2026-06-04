using System.Text.RegularExpressions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Elements;

public sealed class PageNumberElement : IBuilderElement
{
    private readonly string _format;
    private float?  _fontSize;
    private string? _color;
    private HorizontalAlignment _align = HorizontalAlignment.Left;

    // Supported tokens: {page} = current page number, {total} = total pages
    internal PageNumberElement(string format = "Page {page} of {total}") => _format = format;

    public PageNumberElement FontSize(float s) { _fontSize = s;   return this; }
    public PageNumberElement Color(string hex) { _color    = hex; return this; }
    public PageNumberElement AlignLeft()       { _align = HorizontalAlignment.Left;   return this; }
    public PageNumberElement AlignCenter()     { _align = HorizontalAlignment.Center; return this; }
    public PageNumberElement AlignRight()      { _align = HorizontalAlignment.Right;  return this; }

    public void Render(IContainer container)
    {
        var aligned = _align switch
        {
            HorizontalAlignment.Center => container.AlignCenter(),
            HorizontalAlignment.Right  => container.AlignRight(),
            _                          => container.AlignLeft()
        };

        // Split format into tokens: literal strings and {page}/{total} placeholders
        var tokens = Regex.Split(_format, @"(\{page\}|\{total\})");

        aligned.Text(text =>
        {
            foreach (var token in tokens)
            {
                if (token == "{page}")
                {
                    var n = text.CurrentPageNumber();
                    if (_fontSize.HasValue) n.FontSize(_fontSize.Value);
                    if (_color is not null) n.FontColor(_color);
                }
                else if (token == "{total}")
                {
                    var n = text.TotalPages();
                    if (_fontSize.HasValue) n.FontSize(_fontSize.Value);
                    if (_color is not null) n.FontColor(_color);
                }
                else if (token.Length > 0)
                {
                    var s = text.Span(token);
                    if (_fontSize.HasValue) s.FontSize(_fontSize.Value);
                    if (_color is not null) s.FontColor(_color);
                }
            }
        });
    }

    private enum HorizontalAlignment { Left, Center, Right }
}
