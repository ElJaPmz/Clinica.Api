using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.DTOs.HistorialCita
{
    public class HistorialCitaDto
    {
        public int Id_Historial { get; set; }

        public int Id_Cita { get; set; }

        public int Id_Usuario { get; set; }

        public string Accion { get; set; } = string.Empty;

        public DateTime Fecha_Hora { get; set; }

        public string Comentario { get; set; } = string.Empty;
    }
}
