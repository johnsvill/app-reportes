using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppReportes.Clases;
using AppReportes.Models;
using Microsoft.AspNetCore.Mvc;
using cm = System.ComponentModel;

namespace AppReportes.Controllers
{
    public class EspecialidadController : BaseController
    {
        public static List<EspecialidadCLS> lista;

        //Este metodo nos descarga el excel, word y pdf
        public FileResult Exportar(string[] nombreProp, string tipoReporte)
        {
            //string[] cabeceras = { "ID de especialidad", "Nombre", "Descripcion" };
            //string[] nombreProp = { "IdEspecialidad", "Nombre", "Descripcion" };
            if(tipoReporte == "Excel")
            {
                byte[] buffer = ExportarExcelDatos(nombreProp, lista);
                return File(buffer,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if(tipoReporte == "PDF")
            {
                byte[] buffer = ExportarPDFDatos(nombreProp, lista);
                return File(buffer, "application/pdf");
            }
            return null;
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
