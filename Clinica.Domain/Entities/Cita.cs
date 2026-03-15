using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Domain.Entities
{
    public class Cita
    {
        public int IdCita { get; set; }

        public int IdPaciente { get; set; }

        public int IdMedico { get; set; }

        public int IdConsultorio { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public string EstadoCita { get; set; } = null!;

        public string TipoCita { get; set; } = null!;

    }
}
