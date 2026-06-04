using EasyPDF.Sample.Documents;
using EasyPDF.Sample.Models.PosicionCliente;
using EasyPDF.Sample.Services.Interface;
using QuestPDF.Fluent;

namespace EasyPDF.Sample.Services;

public class PosicionClientesServices : IGenerarReporte
{
    public string Generar()
    {
        var posicionData = new PosicionClienteData
        {
            Empresa = "AGRIGOODS",
            Titulo = "Posición Cliente",
            Periodos = "Periodos Junio-Julio",
            Fecha = "02/06/2026",
            Hora = "10:27",
            ClienteFilter = "Clientes Seleccionados",
            ProductoFilter = "",
            Clientes =
            [
                new ClienteSection
                {
                    ClienteId = "25,969",
                    ClienteNombre = "COFCO RESOURCES S.A.",
                    TotalNetoCv = "-14,000,000",
                    Secciones =
                    [
                        new SeccionContratos
                        {
                            Nombre = "Ventas",
                            Total = "28,000,000",
                            Periodos =
                            [
                                new PeriodoGroup
                                {
                                    Periodo = "JUN/2026",
                                    Total = "12,000,000",
                                    Rows =
                                    [
                                        new ContratoRow { Contrato="07714", Fecha="28/08/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="VITOL INC",                    KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08269", Fecha="03/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                  KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="07858", Fecha="31/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="VITOL INC",                    KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="07837", Fecha="22/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",         KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="07853", Fecha="30/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                  KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08274", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                  KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08275", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="ELEMENTA FOODS CHILE S.A.",    KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08293", Fecha="06/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",         KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    ]
                                },
                                new PeriodoGroup
                                {
                                    Periodo = "JUL/2026",
                                    Total = "16,000,000",
                                    Rows =
                                    [
                                        new ContratoRow { Contrato="08274", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                            KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08275", Fecha="04/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="ELEMENTA FOODS CHILE S.A.",              KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08290", Fecha="06/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="MOLINOS OVERSEAS COMMODITIES S.A.",     KilosCtr="6,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08293", Fecha="06/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",                  KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="07837", Fecha="22/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="AVERE COMMODITIES SA",                  KilosCtr="2,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="07853", Fecha="30/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                           KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08269", Fecha="03/03/2026", Vendedor="COFCO RESOURCES S.A.", Comprador="CARGILL INC",                           KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="07858", Fecha="31/10/2025", Vendedor="COFCO RESOURCES S.A.", Comprador="VITOL INC",                             KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    ]
                                }
                            ]
                        },
                        new SeccionContratos
                        {
                            Nombre = "Compras",
                            Total = "14,000,000",
                            Periodos =
                            [
                                new PeriodoGroup
                                {
                                    Periodo = "JUN/2026",
                                    Total = "7,000,000",
                                    Rows =
                                    [
                                        new ContratoRow { Contrato="07963", Fecha="12/12/2025", Vendedor="BUNGE S.A.",                       Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08164", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY",      Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08165", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY",      Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08171", Fecha="30/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY",      Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08057", Fecha="09/01/2026", Vendedor="BUNGE S.A.",                       Comprador="COFCO RESOURCES S.A.", KilosCtr="3,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUN/2026", Producto="SOYABEAN OIL" },
                                    ]
                                },
                                new PeriodoGroup
                                {
                                    Periodo = "JUL/2026",
                                    Total = "7,000,000",
                                    Rows =
                                    [
                                        new ContratoRow { Contrato="08057", Fecha="09/01/2026", Vendedor="BUNGE S.A.",                       Comprador="COFCO RESOURCES S.A.", KilosCtr="3,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08164", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY",      Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08165", Fecha="29/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY",      Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="08171", Fecha="30/01/2026", Vendedor="AGROGRAIN LTD. SUC. URUGUAY",      Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                        new ContratoRow { Contrato="07963", Fecha="12/12/2025", Vendedor="BUNGE S.A.",                       Comprador="COFCO RESOURCES S.A.", KilosCtr="1,000,000", Precio="0.00", Puerto="TIMBUES", Periodo="JUL/2026", Producto="SOYABEAN OIL" },
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        };

        var posicionDoc = new PosicionClienteDocument(posicionData);
        var posicionPath = "posicion_clientes.pdf";
        posicionDoc.GeneratePdf(posicionPath);

        return posicionPath;
    }
}