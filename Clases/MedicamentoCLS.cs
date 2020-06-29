using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppReportes.Clases
{
    public class MedicamentoCLS
    {
        [Display(Name = "Id de medicamento")]
        public int IdMedicamento { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage ="Ingrese el nombre del medicamento")]
        public string Nombre { get; set; }

        [Display(Name = "Precio de medicamento")]
        [Required(ErrorMessage = "Ingrese el precio del medicamento")]
        public decimal? Precio { get; set; }

        [Display(Name = "Forma farmaceutica")]
        [Required(ErrorMessage = "Ingrese la forma farmaceutica")]
        public string NombreFormaFarmaceutica { get; set; }

        [Display(Name = "Stock de producto")]
        [Range(0,10000, ErrorMessage ="Debe estar en el rango de 0 a 10,000")]
        [Required(ErrorMessage = "Ingrese el stock del medicamento")]
        public int? Stock { get; set; }

        [Display(Name = "ID de forma")]
        public int? IdFormaFarmaceutica { get; set; }
        //Adicional
        [Display(Name = "Concentración del medicamento")]
        public string Concentracion { get; set; }

        [Display(Name = "Presentación del medicamento")]
        public string Presentacion { get; set; }
    }
}
