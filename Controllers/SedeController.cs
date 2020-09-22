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
        public string Error = "Ha ocurrido una excepción, favor intente de nuevo";

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
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    if (oSedeCLS.NombreSede == "" || oSedeCLS.NombreSede == null)//Filtro sensitivo
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
            }
            catch(Exception e)
            {
                Error = e.Message;
            }
            lista = listaSedes;
            return View(listaSedes);
        }

        public IActionResult Editar(int id)
        {
            SedeCLS oSedeCLS = new SedeCLS();
            try
            {                
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    oSedeCLS = (from sede in db.Sede
                                where sede.Iidsede == id
                                select new SedeCLS
                                {
                                    IdSede = sede.Iidsede,
                                    NombreSede = sede.Nombre,
                                    Direccion = sede.Direccion
                                }).First();
                }
            }
            catch(Exception e)
            {
                Error = e.Message;
            }
            return View(oSedeCLS);
        }

        [HttpPost]
        public IActionResult Guardar(SedeCLS oSedeCLS)
        {
            string NombreVista = "";

            try
            {
                if (oSedeCLS.IdSede == 0) NombreVista = "Agregar";
                else NombreVista = "Editar";

                if(!ModelState.IsValid)
                {
                    return View(NombreVista, oSedeCLS);
                }
                else
                {
                    using(BDHospitalContext db = new BDHospitalContext())
                    {
                        if(oSedeCLS.IdSede != 0)
                        {
                            Sede sede = db.Sede.Where(x => x.Iidsede == oSedeCLS.IdSede).First();
                            sede.Nombre = oSedeCLS.NombreSede;
                            sede.Direccion = oSedeCLS.Direccion;
                            db.SaveChanges();
                        }
                        
                    }
                }
            }
            catch(Exception e)
            {
                Error = e.Message;
            }
            return RedirectToAction("Index");
        }

        //Eliminación lógica
        [HttpPost]
        public IActionResult Eliminar(int IdSede)
        {
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    Sede oSede = db.Sede.Where(x => x.Iidsede == IdSede).First();
                    oSede.Bhabilitado = 0;//Eliminación lógica
                    db.SaveChanges();
                }
            }
            catch(Exception e)
            {
                Error = e.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
