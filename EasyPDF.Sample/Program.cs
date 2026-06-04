using EasyPDF.Factory;
using EasyPDF.Sample.Services;
using EasyPDF.Sample.Services.Interface;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var generator = PdfGeneratorFactory.Create();

IGenerarReporte generarReporte = new PruebaPosicionPDF();;

var path = generarReporte.Generar();

Console.WriteLine(path); 