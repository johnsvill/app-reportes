using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppReportes.Clases;
using AppReportes.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;

namespace AppReportes.Controllers
{
    public class MedicamentoController : Controller
    {
        public string Error = "Ha ocurrido una excepción, favor intente de nuevo";

        public List<SelectListItem> ListarFormaFarmaceutica()
        {            
            List<SelectListItem> selectLists = new List<SelectListItem>();
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
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
            }
            catch(Exception e)
            {
                Error = e.Message;
            }
            return selectLists;
        }

        public IActionResult Agregar()
        {
            //Siempre para pasar un ComboBox a la vista se hace un ViewBag
            ViewBag.ListaFormaFarmaceutica = ListarFormaFarmaceutica();
            return View();
        }

        //Recuperar información para el combo box
        [HttpGet]
        public IActionResult Editar(int id)
        {
            MedicamentoCLS oMedicamentoCLS = new MedicamentoCLS();
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    oMedicamentoCLS = (from m in db.Medicamento
                                       where m.Iidmedicamento == id
                                       select new MedicamentoCLS
                                       {
                                           IdMedicamento = m.Iidmedicamento,
                                           Nombre = m.Nombre,
                                           Concentracion = m.Concentracion,
                                           IdFormaFarmaceutica = m.Iidformafarmaceutica,
                                           Precio = m.Precio,
                                           Stock = m.Stock,
                                           Presentacion = m.Presentacion
                                       }).First();
                }
            }
            catch(Exception e)
            {
                Error = e.Message;
            }
            //Siempre para pasar un ComboBox a la vista se hace un ViewBag
            ViewBag.ListaFormaFarmaceutica = ListarFormaFarmaceutica();
            return View(oMedicamentoCLS);
        }

        //Metodo para editar un combo box
        [HttpPost]
        public IActionResult Agregar(MedicamentoCLS medicamentoCLS)
        {
            string NombreVista = "";
            try
            {
                using(BDHospitalContext db = new BDHospitalContext())
                {
                    if (medicamentoCLS.IdMedicamento == 0) NombreVista = "Agregar";
                    else NombreVista = "Editar";

                    if (!ModelState.IsValid)
                    {
                        //Siempre para pasar un ComboBox a la vista se hace un ViewBag
                        ViewBag.ListaFormaFarmaceutica = ListarFormaFarmaceutica();
                        return View(NombreVista, medicamentoCLS);
                    }
                    else
                    {
                        if(medicamentoCLS.IdMedicamento == 0)
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
                        else
                        {
                            Medicamento m = db.Medicamento
                                .Where(x => x.Iidmedicamento == medicamentoCLS.IdMedicamento).First();
                            m.Nombre = medicamentoCLS.Nombre;
                            m.Concentracion = medicamentoCLS.Concentracion;
                            m.Iidformafarmaceutica = medicamentoCLS.IdFormaFarmaceutica;
                            m.Precio = medicamentoCLS.Precio;
                            m.Stock = medicamentoCLS.Stock;
                            m.Presentacion = medicamentoCLS.Presentacion;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch(Exception)
            {
                return View(NombreVista, medicamentoCLS);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index(MedicamentoCLS medicamentoCLS)
        {
            ViewBag.ListaForma = ListarFormaFarmaceutica();
            List<MedicamentoCLS> listaMedicamento = new List<MedicamentoCLS>();
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    if (medicamentoCLS.IdFormaFarmaceutica == null ||
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
            }
            catch(Exception e)
            {
                Error = e.Message;
            }
            return View(listaMedicamento);
        }

        //Eliminacion fisica
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            try
            {             
                using(BDHospitalContext db = new BDHospitalContext())
                {
                    Medicamento oMedicamento = db.Medicamento.Where(x => x.Iidmedicamento == id).First();
                    db.Medicamento.Remove(oMedicamento);
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
