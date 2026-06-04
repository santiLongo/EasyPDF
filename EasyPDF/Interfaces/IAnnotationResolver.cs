using EasyPDF.Models;

namespace EasyPDF.Interfaces;

public interface IAnnotationResolver
{
    IReadOnlyList<PropertyDescriptor> Resolve(IPdfDocument model);
    PageConfiguration ResolvePageConfiguration(Type modelType);
    PropertyDescriptor? ResolveHeader(IPdfDocument model);
    PropertyDescriptor? ResolveFooter(IPdfDocument model);
}
