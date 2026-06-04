using System.Reflection;
using EasyPDF.Annotations.Content;
using EasyPDF.Annotations.Page;
using EasyPDF.Enums;
using EasyPDF.Interfaces;
using EasyPDF.Models;

namespace EasyPDF.Resolvers;

public sealed class PropertyAnnotationResolver : IAnnotationResolver
{
    public IReadOnlyList<PropertyDescriptor> Resolve(IPdfDocument model)
    {
        return model.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.IsDefined(typeof(PdfElementAttribute), false))
            .Where(p => !p.IsDefined(typeof(PdfIgnoreAttribute), false))
            .Select(p => new PropertyDescriptor(
                p,
                p.GetValue(model),
                p.GetCustomAttributes().Cast<Attribute>()
            ))
            .OrderBy(d => d.GetAttribute<PdfElementAttribute>()!.Order)
            .ToList()
            .AsReadOnly();
    }

    public PropertyDescriptor? ResolveHeader(IPdfDocument model) =>
        ResolveZone<PdfPageHeaderContentAttribute>(model);

    public PropertyDescriptor? ResolveFooter(IPdfDocument model) =>
        ResolveZone<PdfPageFooterContentAttribute>(model);

    public PageConfiguration ResolvePageConfiguration(Type modelType)
    {
        var config = new PageConfiguration();

        var pageSize = modelType.GetCustomAttribute<PdfPageSizeAttribute>();
        if (pageSize is not null) config.PageSize = pageSize.Size;

        var pageColor = modelType.GetCustomAttribute<PdfPageColorAttribute>();
        if (pageColor is not null) config.PageColor = pageColor.Color;

        var pageMargin = modelType.GetCustomAttribute<PdfPageMarginAttribute>();
        if (pageMargin is not null)
        {
            config.MarginValue = pageMargin.Value;
            config.MarginUnit = pageMargin.Unit;
        }

        var defaultFont = modelType.GetCustomAttribute<PdfDefaultFontSizeAttribute>();
        if (defaultFont is not null) config.DefaultFontSize = defaultFont.Size;

        return config;
    }

    private static PropertyDescriptor? ResolveZone<TAttr>(IPdfDocument model)
        where TAttr : Attribute
    {
        var prop = model.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(p => p.IsDefined(typeof(TAttr), false));

        if (prop is null) return null;

        return new PropertyDescriptor(
            prop,
            prop.GetValue(model),
            prop.GetCustomAttributes().Cast<Attribute>()
        );
    }
}
