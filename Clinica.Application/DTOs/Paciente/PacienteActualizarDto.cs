using System;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Application.DTOs.Paciente
{
    public class PacienteActualizarDto
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(19)]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de paciente es obligatorio.")]
        [StringLength(20)]
        public string TipoPaciente { get; set; } = string.Empty;
    }
}