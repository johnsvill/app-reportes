using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppReportes.Clases
{
    public class SedeCLS
    {
        [Display(Name = "Id de sede")]
        public int IdSede { get; set; }
        [Display(Name = "Nombre de la Sede")]
        public string NombreSede { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }   
    }
}
