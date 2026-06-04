using System.Collections;
using System.Reflection;
using EasyPDF.Annotations.Content;
using EasyPDF.Interfaces;
using EasyPDF.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Unit = QuestPDF.Infrastructure.Unit;

namespace EasyPDF.Renderers;

public sealed class TableRenderer : IElementRenderer
{
    private readonly IContainerStyleApplier _containerStyleApplier;
    private readonly ITableColumnResolver   _columnResolver;

    public TableRenderer(IContainerStyleApplier containerStyleApplier, ITableColumnResolver columnResolver)
    {
        _containerStyleApplier = containerStyleApplier;
        _columnResolver        = columnResolver;
    }

    public bool CanRender(PropertyDescriptor descriptor) =>
        descriptor.HasAttribute<PdfTableAttribute>();

    public void Render(IContainer container, PropertyDescriptor descriptor)
    {
        var styled = _containerStyleApplier.Apply(container, descriptor.AllAttributes);
        RenderTable(styled, descriptor, indentLevel: 0);
    }

    // ── Core recursive renderer ───────────────────────────────────────────────

    private void RenderTable(IContainer container, PropertyDescriptor descriptor, int indentLevel)
    {
        var tableAttr = descriptor.GetAttribute<PdfTableAttribute>()!;
        if (descriptor.Value is not IEnumerable enumerable) return;

        var items = enumerable.Cast<object>().ToList();
        if (items.Count == 0) return;

        var itemType = GetItemType(descriptor.Property.PropertyType);
        if (itemType is null) return;

        // Columns that are plain data (properties WITHOUT [PdfTable])
        var columns = _columnResolver.Resolve(itemType);

        // Sub-table properties: List<T> properties WITH [PdfTable] on the row type
        var nestedTableProps = GetNestedTableProperties(itemType);

        container.Column(col =>
        {
            if (!string.IsNullOrEmpty(tableAttr.Title))
            {
                col.Item()
                    .PaddingBottom(4, Unit.Point)
                    .Text(tableAttr.Title)
                    .Bold()
                    .FontSize(13f);
            }

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(def =>
                {
                    foreach (var c in columns)
                        def.RelativeColumn(c.RelativeWidth);
                });

                if (tableAttr.ShowHeader && columns.Count > 0)
                {
                    table.Header(header =>
                    {
                        foreach (var c in columns)
                        {
                            header.Cell()
                                .Background(tableAttr.HeaderBackgroundColor)
                                .Padding(5, Unit.Point)
                                .Text(c.Header ?? c.Property.Name)
                                .Bold()
                                .FontColor(tableAttr.HeaderFontColor);
                        }
                    });
                }

                for (var i = 0; i < items.Count; i++)
                {
                    var item    = items[i];
                    var bgColor = tableAttr.StripedRows && i % 2 == 1
                        ? tableAttr.StripedRowColor
                        : "#FFFFFF";

                    // ── Data cells ────────────────────────────────────────────
                    foreach (var column in columns)
                    {
                        var value = column.Property.GetValue(item)?.ToString() ?? string.Empty;
                        table.Cell()
                            .Background(bgColor)
                            .Padding(5, Unit.Point)
                            .Text(value);
                    }

                    // ── Nested tables (recursive) ─────────────────────────────
                    foreach (var nestedProp in nestedTableProps)
                    {
                        var nestedValue = nestedProp.GetValue(item);
                        if (nestedValue is null) continue;

                        var nestedDescriptor = new PropertyDescriptor(
                            nestedProp,
                            nestedValue,
                            nestedProp.GetCustomAttributes().Cast<Attribute>()
                        );

                        table.Cell()
                            .ColumnSpan((uint)columns.Count)
                            .PaddingLeft((indentLevel + 1) * 16f, Unit.Point)
                            .PaddingBottom(4, Unit.Point)
                            .Element(c => RenderTable(c, nestedDescriptor, indentLevel + 1));
                    }
                }
            });
        });
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static IReadOnlyList<PropertyInfo> GetNestedTableProperties(Type itemType) =>
        itemType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.IsDefined(typeof(PdfTableAttribute), false))
            .ToList()
            .AsReadOnly();

    private static Type? GetItemType(Type collectionType)
    {
        if (collectionType.IsArray)
            return collectionType.GetElementType();

        return collectionType
            .GetInterfaces()
            .Concat(new[] { collectionType })
            .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            .Select(t => t.GetGenericArguments()[0])
            .FirstOrDefault();
    }
}
