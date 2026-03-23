using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Clinica.Application.DTOs.Medico
{
    public class MedicoActualizarDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(15, ErrorMessage = "El Nombre no puede exceder los 15 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(15, ErrorMessage = "El apellido no puede exceder los 15 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es obligatorio.")]
        public int Id_Especialidad { get; set; }

        [Required(ErrorMessage = "El telefono es obligatorio.")]
        public int telefono { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es necesario.")]
        public string Estado { get; set; } = string.Empty;
    }
}
