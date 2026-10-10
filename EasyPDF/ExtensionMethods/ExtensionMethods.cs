using QuestPDF.Infrastructure;
using QuestPDF.Fluent;

namespace EasyPDF.ExtensionMethods;

public static class ExtensionMethods
{
    public static void GeneratePdf(this IDocument document, string fileName, string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
            
        var fullPath = Path.Combine(path, fileName);
            
        using var fileStream = File.Create(fullPath);
        document.GeneratePdf(fileStream);
    }
}