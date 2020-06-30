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

namespace AppReportes.Controllers
{
    public class EspecialidadController : Controller
    {
        public static List<EspecialidadCLS> lista;

        //Este metodo nos descarga el excel
        public FileResult ExportarExcel()
        {
            //string[] cabeceras = { "ID de especialidad", "Nombre", "Descripcion" };
            string[] nombreProp = { "IdEspecialidad", "Nombre", "Descripcion" };
            byte[] buffer = ExportarExcelDatos(nombreProp, lista);   
            return File(buffer, 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //Este metodo nos genera el array de bytes(Genera el excel)
        public byte[] ExportarExcelDatos<T>(string[] nombreProp, List<T> lista)
        {
            using(MemoryStream ms =  new MemoryStream())    
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using(ExcelPackage ep = new ExcelPackage())
                {
                    ep.Workbook.Worksheets.Add("Hoja");

                    ExcelWorksheet ew = ep.Workbook.Worksheets[0];
                    Dictionary<string, string> diccionario = cm.TypeDescriptor
                        .GetProperties(typeof(T))
                        .Cast<cm.PropertyDescriptor>()
                        .ToDictionary(p => p.Name, p => p.DisplayName);

                    for(int i = 0; i < nombreProp.Length; i++)
                    {
                        ew.Cells[1, i + 1].Value = diccionario[nombreProp[i]];
                        ew.Column(i + 1).Width = 50;
                    }
                    int fila = 2;
                    int col = 1;

                    foreach(object item  in lista)
                    {
                        col = 1;
                        foreach(string propiedad in nombreProp)
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
                            
        public IActionResult Index(EspecialidadCLS especialidadCLS)
        {
            List<EspecialidadCLS> listaEspecialidad = new List<EspecialidadCLS>();
            using (BDHospitalContext db = new BDHospitalContext())
            {
                //Condición para filtrar
                if (especialidadCLS.Nombre == null || especialidadCLS.Nombre == " ")
                {
                    listaEspecialidad = (from especialidad in db.Especialidad
                                         where especialidad.Bhabilitado == 1
                                         select new EspecialidadCLS
                                         {
                                             IdEspecialidad = especialidad.Iidespecialidad,
                                             Nombre = especialidad.Nombre,
                                             Descripcion = especialidad.Descripcion
                                         }).ToList();
                    ViewBag.NombreEspecialidad = "";             
                }
                else
                {
                    listaEspecialidad = (from especialidad in db.Especialidad
                                         where especialidad.Bhabilitado == 1
                                         && especialidad.Nombre.Contains(especialidadCLS.Nombre)
                                         select new EspecialidadCLS
                                         {
                                             IdEspecialidad = especialidad.Iidespecialidad,
                                             Nombre = especialidad.Nombre,
                                             Descripcion = especialidad.Descripcion
                                         }).ToList();
                    ViewBag.NombreEspecialidad = especialidadCLS.Nombre;//Para guardar la busqueda
                }
            }
            lista = listaEspecialidad;
            return View(listaEspecialidad);
        } 

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(EspecialidadCLS especialidadCLS)
        {
            try
            {
                using(BDHospitalContext db = new BDHospitalContext())
                {
                    if(!ModelState.IsValid)
                    {
                        return View(especialidadCLS);
                    }
                    else
                    {
                        Especialidad objeto = new Especialidad
                        {
                            Nombre = especialidadCLS.Nombre,
                            Descripcion = especialidadCLS.Descripcion,
                            Bhabilitado = 1
                        };
                        db.Especialidad.Add(objeto);
                        db.SaveChanges();
                    }
                }
            }
            catch(Exception)
            {
                return View(especialidadCLS);
            }
            return RedirectToAction("Index");
        }
    }
}
