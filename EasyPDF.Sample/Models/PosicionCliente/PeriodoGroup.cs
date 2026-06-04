namespace EasyPDF.Sample.Models.PosicionCliente;

public sealed class PeriodoGroup
{
    public string Periodo { get; set; } = "";
    public List<ContratoRow> Rows { get; set; } = new();
    public string Total { get; set; } = "";
}
