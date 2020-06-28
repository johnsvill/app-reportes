using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppReportes.Clases;
using AppReportes.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppReportes.Controllers
{
    public class EspecialidadController : Controller
    {
        public IActionResult Index(EspecialidadCLS especialidadCLS)
        {
            List<EspecialidadCLS> listaEspecialidad = new List<EspecialidadCLS>();
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if(especialidadCLS.Nombre == null || especialidadCLS.Nombre == " ")//Condición para filtrar
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
            return View(listaEspecialidad);
        } 
    }
}
