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
    public class PersonaController : Controller
    {
        public void LlenarSexo()
        {
            List<SelectListItem> listaSexo = new List<SelectListItem>();//ComboBox
            using (BDHospitalContext db = new BDHospitalContext())
            {
                listaSexo = (from s in db.Sexo
                             where s.Bhabilitado == 1
                             select new SelectListItem
                             {
                                 Text = s.Nombre,
                                 Value = s.Iidsexo.ToString()
                             }).ToList();
                listaSexo.Insert(0, new SelectListItem
                {
                    Text = "--Selecionar--",
                    Value = ""
                });
            }
            ViewBag.ListaSexo = listaSexo;
        }

        public IActionResult Index(PersonaCLS personaCLS)
        {
            List<PersonaCLS> listaPersona = new List<PersonaCLS>();
            LlenarSexo();
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if(personaCLS.IdSexo == null || personaCLS.IdSexo == 0)
                {
                    listaPersona = (from persona in db.Persona
                                    join sexo in db.Sexo
                                    on persona.Iidsexo equals sexo.Iidsexo
                                    where persona.Bhabilitado == 1
                                    select new PersonaCLS
                                    {
                                        IdPersona = persona.Iidpersona,
                                        NombreCompleto = persona.Nombre + " " +
                                        persona.Appaterno + " " + persona.Apmaterno,
                                        Email = persona.Email,
                                        NombreSexo = sexo.Nombre
                                    }).ToList();
                }
               else
                {
                    listaPersona = (from p in db.Persona
                                    join s in db.Sexo
                                    on p.Iidsexo equals s.Iidsexo
                                    where p.Bhabilitado == 1
                                    && p.Iidsexo == personaCLS.IdSexo
                                    select new PersonaCLS
                                    {
                                        IdPersona = p.Iidpersona,
                                        NombreCompleto = p.Nombre + " " +
                                        p.Appaterno + " " + p.Apmaterno,
                                        Email = p.Email,
                                        NombreSexo = s.Nombre
                                    }).ToList();
                }
            }
            return View(listaPersona);
        }

        public IActionResult Agregar()
        {
            LlenarSexo();
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(PersonaCLS oPersonaCLS)
        {
            LlenarSexo();
            try
            {
                if(!ModelState.IsValid)
                {
                    using (BDHospitalContext db = new BDHospitalContext())
                    {
                        Persona oPersona = new Persona();
                        oPersona.Nombre = oPersonaCLS.Nombre;
                        oPersona.Appaterno = oPersonaCLS.ApPaterno;
                        oPersona.Appaterno = oPersonaCLS.ApMaterno;
                        oPersona.Telefonocelular = oPersonaCLS.NumeroTelefono;
                        oPersona.Email = oPersonaCLS.Email;
                        oPersona.Fechanacimiento = oPersonaCLS.FechaNacimiento;
                        oPersona.Iidsexo = oPersonaCLS.IdSexo;
                        oPersona.Bhabilitado = 1;

                        db.Add(oPersona);
                        db.SaveChanges();
                    }
                }
                else
                {
                    return View(oPersonaCLS);
                }
            }
            catch(Exception e)
            {
                return View(oPersonaCLS);
            }
            return RedirectToAction("Index");
        }
    }
}
