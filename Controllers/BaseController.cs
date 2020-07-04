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
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

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

                    if(nombreProp != null && nombreProp.Length > 0)//Validar checks nulos
                    {
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
                    }                  
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

                    if (nombreProp != null && nombreProp.Length > 0)//Validar checks nulos
                    {
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
                    }                       
                    ep.SaveAs(ms);
                    byte[] buffer = ms.ToArray();
                    return buffer;
                }
            }
        }

        public byte[] ExportarWordDatos<T>(string[] nombreProp, List<T> lista)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WordDocument word = new WordDocument();
                WSection wSection = word.AddSection() as WSection;
                wSection.PageSetup.Margins.All = 72;
                wSection.PageSetup.PageSize = new Syncfusion.Drawing.SizeF(612, 792);
                IWParagraph paragraph = wSection.AddParagraph();
                paragraph.ApplyStyle("Normal");
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                WTextRange textRange = paragraph.AppendText("Reporte en Word") as WTextRange;
                textRange.CharacterFormat.FontSize = 20f;
                textRange.CharacterFormat.FontName = "Calibri";
                textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Blue;

                if (nombreProp != null && nombreProp.Length > 0)//Validar checks nulos
                {
                    //Se crea el documento
                    IWTable table = wSection.AddTable();
                    int numeroColumnas = nombreProp.Length;
                    int nFilas = lista.Count;
                    table.ResetCells(nFilas + 1, numeroColumnas);
                    //Obtener cabeceras de las propiedades de la clase
                    Dictionary<string, string> diccionario = cm.TypeDescriptor
                            .GetProperties(typeof(T))
                            .Cast<cm.PropertyDescriptor>()
                            .ToDictionary(p => p.Name, p => p.DisplayName);

                    for (int i = 0; i < nombreProp.Length; i++)
                    {
                        table[0, i].AddParagraph().AppendText(diccionario[nombreProp[i]]);
                    }
                    int fila = 1;
                    int col = 0;

                    foreach (object item in lista)
                    {
                        col = 0;
                        foreach (string propiedad in nombreProp)
                        {
                            table[fila, col].AddParagraph().AppendText(
                                item.GetType().GetProperty(propiedad).GetValue(item).ToString()
                                );
                            col++;
                        }
                        fila++;
                    }
                }
                    
                word.Save(ms, FormatType.Docx);
                return ms.ToArray();
            }
        }
    }
}
