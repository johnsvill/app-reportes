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
        public string Nombre { get; set; }
        [Display(Name = "Precio de medicamento")]
        public decimal Precio { get; set; }
        [Display(Name = "Forma farmaceutica")]   
        public string NombreFormaFarmaceutica { get; set; }
        [Display(Name = "Stock de producto")]
        public int Stock { get; set; }
    }
}
