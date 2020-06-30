using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppReportes.Clases;
using AppReportes.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppReportes.Controllers
{
    public class MedicamentoController : Controller
    {
        public List<SelectListItem> ListarFormaFarmaceutica()
        {
            List<SelectListItem> selectLists = new List<SelectListItem>();
            using(BDHospitalContext db = new BDHospitalContext())
            {
                selectLists = (from ff in db.FormaFarmaceutica
                               where ff.Bhabilitado == 1
                               select new SelectListItem
                               {
                                   Text = ff.Nombre,
                                   Value = ff.Iidformafarmaceutica.ToString()
                               }).ToList();
                selectLists.Insert(0, new SelectListItem
                {
                    Text = "--Seleccione--",
                    Value = ""
                });
            }
            return selectLists;
        }

        public IActionResult Agregar()
        {
            //Siempre para pasar un ComboBox a la vista se hace un ViewBag
            ViewBag.ListaFormaFarmaceutica = ListarFormaFarmaceutica();
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(MedicamentoCLS medicamentoCLS)
        {
            try
            {
                using(BDHospitalContext db = new BDHospitalContext())
                {
                    if(!ModelState.IsValid)
                    {
                        //Siempre para pasar un ComboBox a la vista se hace un ViewBag
                        ViewBag.ListaFormaFarmaceutica = ListarFormaFarmaceutica();
                        return View(medicamentoCLS);
                    }
                    else
                    {
                        Medicamento m = new Medicamento();
                        m.Nombre = medicamentoCLS.Nombre;
                        m.Concentracion = medicamentoCLS.Concentracion;
                        m.Iidformafarmaceutica = medicamentoCLS.IdFormaFarmaceutica;
                        m.Precio = medicamentoCLS.Precio;
                        m.Stock = medicamentoCLS.Stock;
                        m.Presentacion = medicamentoCLS.Presentacion;
                        m.Bhabilitado = 1;
                        db.Medicamento.Add(m);
                        db.SaveChanges();
                    }
                }
            }
            catch(Exception)
            {

            }
            return RedirectToAction("Index");
        }

        public IActionResult Index(MedicamentoCLS medicamentoCLS)
        {
            ViewBag.ListaForma = ListarFormaFarmaceutica();
            List<MedicamentoCLS> listaMedicamento = new List<MedicamentoCLS>();
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if(medicamentoCLS.IdFormaFarmaceutica == null ||
                    medicamentoCLS.IdFormaFarmaceutica == 0)//Validacion ? en propiedades de tipo int y decimal
                {
                    listaMedicamento = (from m in db.Medicamento
                                        join ff in db.FormaFarmaceutica
                                        on m.Iidformafarmaceutica equals ff.Iidformafarmaceutica
                                        where m.Bhabilitado == 1
                                        select new MedicamentoCLS
                                        {
                                            IdMedicamento = m.Iidmedicamento,
                                            Nombre = m.Nombre,
                                            Precio = (decimal)m.Precio,
                                            Stock = (int)m.Stock,
                                            NombreFormaFarmaceutica = ff.Nombre
                                        }).ToList();
                }
                else
                {
                    listaMedicamento = (from m in db.Medicamento
                                        join ff in db.FormaFarmaceutica
                                        on m.Iidformafarmaceutica equals ff.Iidformafarmaceutica
                                        where m.Bhabilitado == 1
                                        && m.Iidformafarmaceutica == medicamentoCLS.IdFormaFarmaceutica
                                        select new MedicamentoCLS
                                        {
                                            IdMedicamento = m.Iidmedicamento,
                                            Nombre = m.Nombre,
                                            Precio = (decimal)m.Precio,
                                            Stock = (int)m.Stock,
                                            NombreFormaFarmaceutica = ff.Nombre
                                        }).ToList();
                }
            }
            return View(listaMedicamento);
        }
    }
}
