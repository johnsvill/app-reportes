using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppReportes.Clases;
using AppReportes.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppReportes.Controllers
{
    public class EspecialidadController : BaseController
    {
        public static List<EspecialidadCLS> lista;
        public string Error = "Ha ocurrido un error en tiempo de ejecución, por favor espere";

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
            else if(tipoReporte == "Word")
            {
                byte[] buffer = ExportarWordDatos(nombreProp, lista);
                return File(buffer, 
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }           
            return null;
        }

        public string ExportarDatosPDF(string[] nombreProp)
        {
            byte[] buffer = ExportarPDFDatos(nombreProp, lista);
            string cadena = Convert.ToBase64String(buffer);
            cadena = "data:application/pdf;base64," + cadena;
            return cadena;
        }
        
        public IActionResult Index(EspecialidadCLS especialidadCLS)
        {
            List<EspecialidadCLS> listaEspecialidad = new List<EspecialidadCLS>();
            using (BDHospitalContext db = new BDHospitalContext())
            {                
                try
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
                catch (Exception e)
                {
                    Error = e.Message;
                    throw e;
                }                
            }
            lista = listaEspecialidad;
            return View(listaEspecialidad);
        } 

        public IActionResult Agregar()
        {
            return View();
        }

        //Eliminación lógica
        [HttpPost]
        public IActionResult Eliminar(int IdEspecialidad)
        {           
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    Especialidad oEspecialidad = db.Especialidad
                        .Where(x => x.Iidespecialidad == IdEspecialidad).First();
                    oEspecialidad.Bhabilitado = 0;//Se cambia de estado y no se elimina
                    db.SaveChanges();
                }
            }
            catch(Exception e)
            {
                Error = e.Message;
                throw e;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Agregar(EspecialidadCLS especialidadCLS)
        {
            string NombreVista = "";
            int nveces = 0;
            try
            {
                if (especialidadCLS.IdEspecialidad == 0) NombreVista = "Agregar";
                else NombreVista = "Editar";

                using(BDHospitalContext db = new BDHospitalContext())
                {
                    //Se analiza cuantas veces el nombre de la especialidad se repite en la DB
                    if(especialidadCLS.IdEspecialidad == 0)//Solo afecta al metodo Agregar                        
                    nveces = db.Especialidad
                        .Where(x => x.Nombre.ToUpper().Trim() 
                        == especialidadCLS.Nombre.ToUpper().Trim()).Count();//Para que la comparacion sea todo en mayuscula en DB

                    if(!ModelState.IsValid || nveces >= 1)
                    {
                        if (nveces >= 1) especialidadCLS.MensajeError = "El nombre de la especialidad ya existe";
                        return View(NombreVista, especialidadCLS);
                    }
                    else
                    {
                        if(especialidadCLS.IdEspecialidad == 0)
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
                        else
                        {
                            Especialidad objeto = db.Especialidad
                                .Where(p => p.Iidespecialidad == especialidadCLS.IdEspecialidad).First();
                            objeto.Nombre = especialidadCLS.Nombre;
                            objeto.Descripcion = especialidadCLS.Descripcion;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                return View(NombreVista, especialidadCLS);
                throw e;
            }          
            return RedirectToAction("Index");
        }
           
        [HttpGet]
        public IActionResult Editar(int id)
        {
            EspecialidadCLS oEspecialidadCLS = new EspecialidadCLS();
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    oEspecialidadCLS = (from e in db.Especialidad
                                        where e.Iidespecialidad == id
                                        select new EspecialidadCLS
                                        {
                                            IdEspecialidad = e.Iidespecialidad,
                                            Nombre = e.Nombre,
                                            Descripcion = e.Descripcion
                                        }).First();
                }
            }
            catch(Exception e)
            {
                Error = e.Message;
                throw e;
            }
            return View(oEspecialidadCLS);
        }
    }
}
