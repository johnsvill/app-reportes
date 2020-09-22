using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppReportes.Clases
{
    public class EspecialidadCLS
    {
        [Display(Name ="Id de especialidad")]//Formulario
        [DisplayName("ID de la especialidad")]//Cabecera excel
        public int IdEspecialidad { get; set; }
        [Display(Name = "Nombre de especialidad")]
        [DisplayName("Nombre de la especialidad")]//Cabecera excel
        [Required(ErrorMessage ="Ingrese nombre de la especialidad")]
        public string Nombre { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Ingrese la descripción de la especialidad")]
        [DisplayName("Descripción de la especialidad")]//Cabecera excel
        public string Descripcion { get; set; }   
        public string MensajeError { get; set; }
    }
}

