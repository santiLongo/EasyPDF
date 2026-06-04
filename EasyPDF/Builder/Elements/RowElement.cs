using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EasyPDF.Builder.Elements;

public sealed class RowElement : IBuilderElement
{
    private readonly List<RowColumnConfig> _columns = new();

    public RowElement Auto(Action<SectionBuilder> configure)
    {
        var section = new SectionBuilder();
        configure(section);
        _columns.Add(new RowColumnConfig(RowColumnType.Auto, section));
        return this;
    }

    public RowElement Fill(Action<SectionBuilder> configure)
    {
        var section = new SectionBuilder();
        configure(section);
        _columns.Add(new RowColumnConfig(RowColumnType.Fill, section));
        return this;
    }

    public RowElement Fixed(float points, Action<SectionBuilder> configure)
    {
        var section = new SectionBuilder();
        configure(section);
        _columns.Add(new RowColumnConfig(RowColumnType.Fixed, points, section));
        return this;
    }

    public void Render(IContainer container)
    {
        container.Row(row =>
        {
            foreach (var col in _columns)
            {
                IContainer cell = col.Type switch
                {
                    RowColumnType.Auto  => row.AutoItem(),
                    RowColumnType.Fixed => row.ConstantItem(col.Size, Unit.Point),
                    _                  => row.RelativeItem()
                };

                col.Section.Render(cell);
            }
        });
    }
}

internal enum RowColumnType { Auto, Fill, Fixed }

internal sealed class RowColumnConfig
{
    internal RowColumnType Type    { get; }
    internal float         Size    { get; }
    internal SectionBuilder Section { get; }

    internal RowColumnConfig(RowColumnType type, SectionBuilder section, float size = 0f)
    {
        Type    = type;
        Size    = size;
        Section = section;
    }

    internal RowColumnConfig(RowColumnType type, float size, SectionBuilder section)
    {
        Type    = type;
        Size    = size;
        Section = section;
    }
}
