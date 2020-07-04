using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppReportes.Clases
{
    public class SedeCLS
    {
        [DisplayName("ID de la sede")]
        [Display(Name = "Id de sede")]
        public int IdSede { get; set; }

        [DisplayName("Nombre de la sede")]
        [Display(Name = "Nombre de la Sede")]
        public string NombreSede { get; set; }

        [Display(Name = "Dirección")]
        [DisplayName("Dirección de la sede")]
        public string Direccion { get; set; }   
    }
}
