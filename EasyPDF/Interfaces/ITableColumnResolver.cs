using EasyPDF.Models;

namespace EasyPDF.Interfaces;

public interface ITableColumnResolver
{
    IReadOnlyList<ColumnInfo> Resolve(Type itemType);
}
