
using System.ComponentModel.DataAnnotations;

namespace Clinica.Application.DTOs.Especialidad
{
    public class EspecialidadCrearDto
    {
        [Required(ErrorMessage = "El nombre de la especialidad es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre_Especialidad { get; set; } = string.Empty;
    }
}
