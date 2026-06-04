namespace EasyPDF.Sample.Models.PosicionCliente;

public sealed class SeccionContratos
{
    public string Nombre { get; set; } = "";
    public List<PeriodoGroup> Periodos { get; set; } = new();
    public string Total { get; set; } = "";
}
