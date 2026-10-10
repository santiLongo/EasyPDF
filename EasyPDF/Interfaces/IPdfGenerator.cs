namespace EasyPDF.Interfaces;

public interface IPdfGenerator
{
    byte[] Generate(IPdfDocument model);
    void GenerateToFile(IPdfDocument model, string fileName);
    void GenerateToFile(IPdfDocument model, string fileName, string path);
    Stream GenerateToStream(IPdfDocument model);
}
