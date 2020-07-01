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
        public IActionResult Index(PaginaCLS paginaCLS)
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if(paginaCLS.Mensaje == null || paginaCLS.Mensaje == "")
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
                    ViewBag.Mensaje = "";
                }
                else
                {
                    listaPagina = (from pagina in db.Pagina
                                   where pagina.Bhabilitado == 1
                                   && pagina.Mensaje.Contains(paginaCLS.Mensaje)
                                   select new PaginaCLS
                                   {
                                       IdPagina = pagina.Iidpagina,
                                       Mensaje = pagina.Mensaje,
                                       Accion = pagina.Accion,
                                       Controlador = pagina.Controlador
                                   }).ToList();
                    ViewBag.Mensaje = paginaCLS.Mensaje;
                }
            }
            return View(listaPagina);  
        }

        //Mostrar el formulario
        [HttpGet]
        public IActionResult Agregar()
        {
            return View();
        }

        //Realiza la inserción de datos
        [HttpPost]
        public IActionResult Agregar(PaginaCLS oPaginaCLS)
        {
            try
            {
                using(BDHospitalContext db = new BDHospitalContext())
                {
                    if(!ModelState.IsValid)
                    {
                        return View(oPaginaCLS);
                    }
                    else
                    {
                        Pagina oPagina = new Pagina();
                        oPagina.Mensaje = oPaginaCLS.Mensaje;
                        oPagina.Controlador = oPaginaCLS.Controlador;
                        oPagina.Accion = oPaginaCLS.Controlador;
                        oPagina.Bhabilitado = 1;
                        db.Pagina.Add(oPagina);
                        db.SaveChanges();
                    }                   
                }
            }
            catch(Exception)
            {
                return View(oPaginaCLS);
            }
            return RedirectToAction("Index");
        }
    }
}
