using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppReportes.Clases
{
    public class TipoUsuarioCLS
    {
        [Display(Name ="ID de usuario")]
        public int IdTipoUser { get; set; }
        [Display(Name = "Puesto de usuario")]
        public string Nombre { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
    }
}
