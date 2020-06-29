using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppReportes.Clases;
using AppReportes.Models;

namespace AppReportes.Controllers
{
    public class TipoUsuarioController : Controller
    {
        //Filtrado múltiple
        public IActionResult Index(TipoUsuarioCLS tipoUsuarioCLS)
        {
            List<TipoUsuarioCLS> listaUser = new List<TipoUsuarioCLS>();
            using (BDHospitalContext db = new BDHospitalContext())
            {
                listaUser = (from tu in db.TipoUsuario
                             where tu.Bhabilitado == 1
                             select new TipoUsuarioCLS
                             {
                                 IdTipoUser = tu.Iidtipousuario,
                                 Nombre = tu.Nombre,
                                 Descripcion = tu.Descripcion,                              
                             }).ToList();
                if(tipoUsuarioCLS.Nombre == null && tipoUsuarioCLS.Descripcion == null
                    && tipoUsuarioCLS.IdTipoUser == 0)
                {
                    ViewBag.Nombre = "";
                    ViewBag.Descripcion = "";
                    ViewBag.IdTipoUser = 0;
                }
                else
                {
                    if(tipoUsuarioCLS.Nombre != null)
                    {
                        listaUser = listaUser
                            .Where(x => x.Nombre.Contains(tipoUsuarioCLS.Nombre)).ToList();
                    }
                    if (tipoUsuarioCLS.Descripcion != null)
                    {
                        listaUser = listaUser
                            .Where(x => x.Descripcion.Contains(tipoUsuarioCLS.Descripcion)).ToList();
                    }
                    if (tipoUsuarioCLS.IdTipoUser != 0)
                    {
                        listaUser = listaUser
                            .Where(x => x.IdTipoUser == tipoUsuarioCLS.IdTipoUser).ToList();
                    }
                    //Para guardar la búsqueda
                    ViewBag.Nombre = tipoUsuarioCLS.Nombre;
                    ViewBag.Descripcion = tipoUsuarioCLS.Descripcion;
                    ViewBag.IdTipoUser = tipoUsuarioCLS.IdTipoUser;
                }
            }
            return View(listaUser);
        }
    }
}
