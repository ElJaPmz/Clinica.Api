using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Domain.Entities
{
    public class Recordatorio
    {
        public int IdRecordatorio { get; set; }

        public int IdCita { get; set; }

        public string TipoNotificacion { get; set; } = null!;

        public DateTime FechaEnvio { get; set; }

        public string Estado { get; set; } = null!;
    }
}
