using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppReportes.Clases;
using AppReportes.Models;

namespace AppReportes.Controllers
{
    public class SedeController : BaseController
    {
        public static List<SedeCLS> lista;

        //Este metodo nos descarga el excel, word y pdf
        public FileResult Exportar(string[] nombreProp, string tipoReporte)
        {
            //string[] cabeceras = { "ID de especialidad", "Nombre", "Descripcion" };
            //string[] nombreProp = { "IdEspecialidad", "Nombre", "Descripcion" };
            if (tipoReporte == "Excel")
            {
                byte[] buffer = ExportarExcelDatos(nombreProp, lista);
                return File(buffer,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (tipoReporte == "PDF")
            {
                byte[] buffer = ExportarPDFDatos(nombreProp, lista);
                return File(buffer,
                    "application/pdf");
            }
            else if (tipoReporte == "Word")
            {
                byte[] buffer = ExportarWordDatos(nombreProp, lista);
                return File(buffer,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            return null;
        }

        public IActionResult Index(SedeCLS oSedeCLS)
        {
            List<SedeCLS> listaSedes = new List<SedeCLS>();
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if(oSedeCLS.NombreSede == "" || oSedeCLS.NombreSede == null)//Filtro sensitivo
                {
                    listaSedes = (from sede in db.Sede
                                  where sede.Bhabilitado == 1
                                  select new SedeCLS
                                  {
                                      IdSede = sede.Iidsede,
                                      NombreSede = sede.Nombre,
                                      Direccion = sede.Direccion
                                  }).ToList();
                    ViewBag.NombreSede = "";
                }
                else
                {
                    listaSedes = (from sede in db.Sede
                                  where sede.Bhabilitado == 1
                                  && sede.Nombre.Contains(oSedeCLS.NombreSede)
                                  select new SedeCLS
                                  {
                                      IdSede = sede.Iidsede,
                                      NombreSede = sede.Nombre,
                                      Direccion = sede.Direccion
                                  }).ToList();
                    ViewBag.NombreSede = oSedeCLS.NombreSede;
                }
            }
            lista = listaSedes;
            return View(listaSedes);
        }
    }
}
