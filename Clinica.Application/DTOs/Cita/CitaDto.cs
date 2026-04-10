using System;

namespace Clinica.Application.DTOs.Cita
{
    public class CitaDto
    {
        public int IdCita { get; set; }

        // IDs originales
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdConsultorio { get; set; }

        // --- Campos de información extraídos de las otras tablas ---
        public string? NombreCompletoPaciente { get; set; }
        public string? NombreCompletoMedico { get; set; }
        public string? InfoConsultorio { get; set; } // Aquí pondremos el número y descripción
        // -----------------------------------------------------------

        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string EstadoCita { get; set; } = null!;
        public string TipoCita { get; set; } = null!;
    }
}
