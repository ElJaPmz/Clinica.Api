using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Application.DTOs.Cita
{
    public class CitaCrearDto : IValidatableObject
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

        // Lógica de validación personalizada
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HoraInicio >= HoraFin)
            {
                yield return new ValidationResult(
                    "La hora de fin debe ser mayor a la hora de inicio.",
                    new[] { nameof(HoraInicio), nameof(HoraFin) }
                );
            }

            // Opcional: Validar que la cita sea en el futuro
            if (Fecha.Date < DateTime.Now.Date)
            {
                yield return new ValidationResult(
                    "No se pueden programar citas para fechas pasadas.",
                    new[] { nameof(Fecha) }
                );
            }
        }
    }
}
