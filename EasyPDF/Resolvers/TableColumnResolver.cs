using System.Reflection;
using System.Text.RegularExpressions;
using EasyPDF.Annotations.Content;
using EasyPDF.Annotations.Table;
using EasyPDF.Interfaces;
using EasyPDF.Models;

namespace EasyPDF.Resolvers;

public sealed class TableColumnResolver : ITableColumnResolver
{
    public IReadOnlyList<ColumnInfo> Resolve(Type itemType)
    {
        return itemType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .Where(p => !p.IsDefined(typeof(PdfColumnIgnoreAttribute), false))
            .Where(p => !p.IsDefined(typeof(PdfTableAttribute), false))
            .Select(p =>
            {
                var colAttr = p.GetCustomAttribute<PdfColumnAttribute>();
                return new ColumnInfo
                {
                    Property = p,
                    Header = colAttr?.Header ?? SplitCamelCase(p.Name),
                    RelativeWidth = colAttr?.RelativeWidth ?? 1f,
                    Order = colAttr?.Order ?? 0,
                    Attributes = p.GetCustomAttributes().Cast<Attribute>().ToList().AsReadOnly()
                };
            })
            .OrderBy(c => c.Order)
            .ToList()
            .AsReadOnly();
    }

    private static string SplitCamelCase(string name) =>
        Regex.Replace(name, "([A-Z])", " $1").Trim();
}
