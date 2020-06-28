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
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Sexo")]
        public string NombreSexo { get; set; }
        [Display(Name = "ID del sexo")]
        public int IdSexo { get; set; }
    }
}
