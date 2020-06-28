using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppReportes.Clases;
using AppReportes.Models;

namespace AppReportes.Controllers
{
    public class MedicamentoController : Controller
    {
        public IActionResult Index()
        {
            List<MedicamentoCLS> listaMedicamento = new List<MedicamentoCLS>();
            using (BDHospitalContext db = new BDHospitalContext())
            {
                listaMedicamento = (from m in db.Medicamento
                                    join ff in db.FormaFarmaceutica
                                    on m.Iidformafarmaceutica equals ff.Iidformafarmaceutica
                                    select new MedicamentoCLS
                                    {
                                        IdMedicamento = m.Iidmedicamento,
                                        Nombre = m.Nombre,
                                        Precio = (decimal)m.Precio,
                                        Stock = (int)m.Stock,
                                        NombreFormaFarmaceutica = ff.Nombre
                                    }).ToList();
            }
            return View(listaMedicamento);
        }
    }
}
