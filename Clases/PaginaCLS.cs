using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppReportes.Clases
{
    public class PaginaCLS
    {
        [Display(Name = "Id de página")]
        public int IdPagina { get; set; }
        [Display(Name = "Mensaje")]
        public string Mensaje { get; set; }
        [Display(Name = "Nombre de la acción")]
        public string Accion { get; set; }
        [Display(Name = "Nombre del controlador")]
        public string Controlador { get; set; }
    }
}
