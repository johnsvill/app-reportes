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
        [Required(ErrorMessage ="Debe ingresar un mensaje")]
        public string Mensaje { get; set; }
        [Display(Name = "Nombre de la acción")]
        [Required(ErrorMessage = "Debe ingresar una acción")]
        public string Accion { get; set; }
        [Display(Name = "Nombre del controlador")]
        public string Controlador { get; set; }
    }
}
