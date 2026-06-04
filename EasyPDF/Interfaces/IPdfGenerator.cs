namespace EasyPDF.Interfaces;

public interface IPdfGenerator
{
    byte[] Generate(IPdfDocument model);
    void GenerateToFile(IPdfDocument model, string filePath);
    Stream GenerateToStream(IPdfDocument model);
}
