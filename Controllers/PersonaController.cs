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
        public string Error = "Ha ocurrido un error en tiempo de ejecución, por favor espere";

        public void LlenarSexo()
        {
            List<SelectListItem> listaSexo = new List<SelectListItem>();//ComboBox
            try
            {
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
                        Text = "--Seleccionar--",
                        Value = ""
                    });
                }
            }
            catch(Exception e)
            {
                Error = e.Message;
                throw e;
            }
            ViewBag.ListaSexo = listaSexo;
        }

        public IActionResult Index(PersonaCLS personaCLS)
        {
            List<PersonaCLS> listaPersona = new List<PersonaCLS>();
            try
            {
                LlenarSexo();
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    //Validar para que soporte enteros nulos, ? en propiedad int
                    if (personaCLS.IdSexo == null || personaCLS.IdSexo == 0)
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
            }
            catch(Exception e)
            {
                Error = e.Message;
                throw e;
            }
            return View(listaPersona);
        }

        public IActionResult Agregar()
        {
            LlenarSexo();
            return View();
        }

        [HttpPost]
        public IActionResult Guardar(PersonaCLS oPersonaCLS)
        {
            string NombreVista = "";
            int nveces = 0;

            if (oPersonaCLS.IdPersona == 0) NombreVista = "Agregar";
            else NombreVista = "Editar";
            LlenarSexo();
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    oPersonaCLS.NombreCompleto = oPersonaCLS.Nombre.ToUpper().Trim() + " " +
                         oPersonaCLS.ApMaterno.ToUpper().Trim() + " " + oPersonaCLS.ApMaterno.ToUpper().Trim();

                    //Solo en el caso que sea Agregar
                    //Validar si el nombre completo ya existe
                    if (oPersonaCLS.IdPersona == 0)
                    {
                        nveces = db.Persona.Where(p => p.Nombre.ToUpper().Trim() + " " + 
                        p.Apmaterno.ToUpper().Trim() + " " + p.Apmaterno.ToUpper().Trim() 
                        == oPersonaCLS.NombreCompleto).Count();
                    }
                    if(!ModelState.IsValid || nveces >= 1)
                    {
                        if(nveces >= 1)
                        {
                            oPersonaCLS.MensajeError = "La persona ya existe";
                        }
                        return View(NombreVista, oPersonaCLS);
                    }
                    else
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
                    
            }
            catch(Exception e)
            {                
                return View(NombreVista, oPersonaCLS);
                throw e;
            }
            return RedirectToAction("Index");
        }

        //Eliminacion logica
        [HttpPost]
        public IActionResult Eliminar(int IdPersona)
        {
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    Persona oPersona = db.Persona.Where(x => x.Iidpersona == IdPersona).First();
                    oPersona.Bhabilitado = 0;
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
    }
}
