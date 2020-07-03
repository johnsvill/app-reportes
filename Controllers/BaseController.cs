using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppReportes.Clases;
using AppReportes.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using OfficeOpenXml;
using cm = System.ComponentModel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace AppReportes.Controllers
{
    public class BaseController : Controller
    {
        //Este metodo nos genera el array de bytes(Genera el pdf)
        public byte[] ExportarPDFDatos<T>(string[] nombreProp, List<T> lista)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Dictionary<string, string> diccionario = cm.TypeDescriptor
                        .GetProperties(typeof(T))
                        .Cast<cm.PropertyDescriptor>()
                        .ToDictionary(p => p.Name, p => p.DisplayName);

                PdfWriter writer = new PdfWriter(ms);
                using (var pdfDoc = new PdfDocument(writer))
                {
                    Document doc = new Document(pdfDoc);
                    Paragraph c1 = new Paragraph("Reporte en PDF");
                    c1.SetFontSize(20);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    doc.Add(c1);

                    //Creamos la tabla
                    Table table = new Table(nombreProp.Length);
                    Cell celda;

                    for (int i = 0; i < nombreProp.Length; i++)
                    {
                        celda = new Cell();
                        celda.Add(new Paragraph(diccionario[nombreProp[i]]));
                        table.AddHeaderCell(celda);
                    }
                    foreach (object item in lista)
                    {
                        foreach (string propiedad in nombreProp)
                        {
                            celda = new Cell();
                            celda.Add(new Paragraph(
                                item.GetType().GetProperty(propiedad).GetValue(item).ToString()
                            ));
                            table.AddCell(celda);
                        }
                    }
                    doc.Add(table);
                    doc.Close();
                    writer.Close();
                }
                return ms.ToArray();
            }
        }

        //Este metodo nos genera el array de bytes(Genera el excel)
        public byte[] ExportarExcelDatos<T>(string[] nombreProp, List<T> lista)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage ep = new ExcelPackage())
                {
                    ep.Workbook.Worksheets.Add("Hoja");

                    ExcelWorksheet ew = ep.Workbook.Worksheets[0];
                    Dictionary<string, string> diccionario = cm.TypeDescriptor
                        .GetProperties(typeof(T))
                        .Cast<cm.PropertyDescriptor>()
                        .ToDictionary(p => p.Name, p => p.DisplayName);

                    for (int i = 0; i < nombreProp.Length; i++)
                    {
                        ew.Cells[1, i + 1].Value = diccionario[nombreProp[i]];
                        ew.Column(i + 1).Width = 50;
                    }
                    int fila = 2;
                    int col = 1;

                    foreach (object item in lista)
                    {
                        col = 1;
                        foreach (string propiedad in nombreProp)
                        {
                            ew.Cells[fila, col].Value =
                                item.GetType().GetProperty(propiedad).GetValue(item).ToString();
                            col++;//Para aumentarle de uno en uno a las filas
                        }
                        fila++;//Para aumentarle de uno en uno a las columnas
                    }
                    ep.SaveAs(ms);
                    byte[] buffer = ms.ToArray();
                    return buffer;
                }
            }
        }
    }
}
