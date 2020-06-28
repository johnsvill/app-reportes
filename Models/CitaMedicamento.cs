﻿using System;
using System.Collections.Generic;

namespace AppReportes.Models
{
    public partial class CitaMedicamento
    {
        public int Iidcitamedicamento { get; set; }
        public int? Iidcita { get; set; }
        public int? Iidmedicamento { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Precio { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual Cita IidcitaNavigation { get; set; }
        public virtual Medicamento IidmedicamentoNavigation { get; set; }
    }
}
