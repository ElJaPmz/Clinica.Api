using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Domain.Entities
{
    public  class HistorialCita
    {
        public int Id_Historial { get; set; } // PK

        public int Id_Cita { get; set; } // FK a Cita

        public int Id_Usuario { get; set; } // FK a Usuario (quien realiza el cambio)

        /// <summary>
        /// Valores permitidos: Crear, Modificar, Cancelar, Reprogramar.
        /// </summary>
        public string Accion { get; set; } = string.Empty;

        public DateTime Fecha_Hora { get; set; }

        public string Comentario { get; set; } = string.Empty;
    }
}
