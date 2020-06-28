using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppReportes.Clases;
using AppReportes.Models;

namespace AppReportes.Controllers
{
    public class SedeController : Controller
    {
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
            return View(listaSedes);
        }
    }
}
