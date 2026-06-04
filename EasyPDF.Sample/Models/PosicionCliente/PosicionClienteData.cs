namespace EasyPDF.Sample.Models.PosicionCliente;

public sealed class PosicionClienteData
{
    public string Empresa { get; set; } = "AGRIGOODS";
    public string Titulo { get; set; } = "Posición Cliente";
    public string Periodos { get; set; } = "";
    public string Fecha { get; set; } = "";
    public string Hora { get; set; } = "";
    public string ClienteFilter { get; set; } = "Clientes Seleccionados";
    public string ProductoFilter { get; set; } = "";
    public List<ClienteSection> Clientes { get; set; } = new();
}
