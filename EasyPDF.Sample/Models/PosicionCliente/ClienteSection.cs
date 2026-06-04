namespace EasyPDF.Sample.Models.PosicionCliente;

public sealed class ClienteSection
{
    public string ClienteId { get; set; } = "";
    public string ClienteNombre { get; set; } = "";
    public List<SeccionContratos> Secciones { get; set; } = new();
    public string TotalNetoCv { get; set; } = "";
}
