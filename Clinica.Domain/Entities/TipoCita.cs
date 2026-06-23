using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Domain.Entities
{
    public class TipoCita
    {
        public int Id_TipoCita { get; set; }
        public string Nombre_TipoCita { get; set; } = string.Empty;
        public int Id_Especialidad { get; set; }

        // Navegación
        public virtual Especialidad Especialidad { get; set; } = null!;
    }
}
