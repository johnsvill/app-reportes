using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppReportes.Clases
{
    public class EspecialidadCLS
    {
        [Display(Name ="Id de especialidad")]
        public int IdEspecialidad { get; set; }
        [Display(Name = "Nombre de especialidad")]
        public string Nombre { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }    
    }
}

