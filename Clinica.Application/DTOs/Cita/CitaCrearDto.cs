using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Clinica.Application.DTOs.Cita
{
    public class CitaCrearDto
    {
        [Required(ErrorMessage = "El ID del Paciente es requerido.")]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "El ID del Médico es requerido.")]
        public int IdMedico { get; set; }

        [Required(ErrorMessage = "El ID del Consultorio es requerido.")]
        public int IdConsultorio { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La Hora de Inicio es requerida.")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La Hora de Fin es requerida.")]
        public TimeSpan HoraFin { get; set; }

        [Required(ErrorMessage = "El Tipo de cita es requerido.")]
        [MaxLength(30, ErrorMessage = "El Tipo de cita no puede exceder los 30 caracteres.")]
        public string TipoCita { get; set; } = string.Empty;
    }
}
