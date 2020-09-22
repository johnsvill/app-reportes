using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppReportes.Clases
{
    public class PersonaCLS
    {
        [Display(Name ="Id de persona")]
        public int IdPersona { get; set; }

        [Display(Name = "Ingrese nombre")]
        [Required(ErrorMessage = "Debe ingresar su nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar su apellido paterno")]
        [Display(Name = "Apellido paterno")]
        public string ApPaterno { get; set; }

        [Display(Name = "Apellido materno")]
        [Required(ErrorMessage = "Debe ingresar su apellido materno")]
        public string ApMaterno { get; set; }

        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage ="Debe ingresar su número de teléfono")]
        [MinLength(8, ErrorMessage ="Debe contener 8 dígitos")]
        [Display(Name = "Número de teléfono")]
        public string NumeroTelefono { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Debe ingresar un correo válido")]
        public string Email { get; set; }

        [DataType(DataType.DateTime, ErrorMessage ="El formato de fecha es incorrecto")]
        [Display(Name ="Fecha de nacimiento")]
        [Required(ErrorMessage ="Debe ingresar su fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Sexo")]
        public string NombreSexo { get; set; }

        [Required(ErrorMessage = "Seleccione una opción")]
        [Display(Name = "Seleccione su sexo")]
        public int? IdSexo { get; set; }

        public string MensajeError { get; set; }
    }
}
