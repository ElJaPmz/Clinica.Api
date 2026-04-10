using System.ComponentModel.DataAnnotations;

namespace Clinica.Application.DTOs.Especialidad
{
    public class EspecialidadActualizarDto
    {
        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        [StringLength(100)]
        public string Nombre_Especialidad { get; set; } = string.Empty;
    }
}