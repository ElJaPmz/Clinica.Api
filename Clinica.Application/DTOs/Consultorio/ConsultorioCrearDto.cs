

using System.ComponentModel.DataAnnotations;

namespace Clinica.Application.DTOs.Consultorio
{
    public class ConsultorioCrearDto
    {
        [Required(ErrorMessage ="El Número del Consultorio es Requerido.")]
        public int NumeroConsultorio { get; set; }

        [Required(ErrorMessage = "La Descripción del Consultorio es Requerida.")]
        [MaxLength(100, ErrorMessage = "La Descripción no puede exceder los 100 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Estado del Consultorio es Requerido.")]
        [MaxLength(50, ErrorMessage = "El Estado no puede exceder los 50 caracteres.")]
        public string Estado { get; set; } = string.Empty;
    }
}
