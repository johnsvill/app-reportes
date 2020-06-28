using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppReportes.Clases;
using AppReportes.Models;

namespace AppReportes.Controllers
{
    public class PaginaController : Controller
    {
        public IActionResult Index()
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (BDHospitalContext db = new BDHospitalContext())
            {
                listaPagina = (from pagina in db.Pagina
                               where pagina.Bhabilitado == 1
                               select new PaginaCLS
                               {
                                   IdPagina = pagina.Iidpagina,
                                   Mensaje = pagina.Mensaje,
                                   Accion = pagina.Accion,
                                   Controlador = pagina.Controlador
                               }).ToList();
            }
            return View(listaPagina);  
        }
    }
}
