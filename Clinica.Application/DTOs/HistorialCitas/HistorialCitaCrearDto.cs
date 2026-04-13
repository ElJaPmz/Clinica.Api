using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Clinica.Application.DTOs.HistorialCita
{
    public class HistorialCitaCrearDto
    {
        [Required(ErrorMessage = "El ID de la Cita es requerido.")]
        public int Id_Cita { get; set; }

        [Required(ErrorMessage = "El ID del Usuario es requerido.")]
        public int Id_Usuario { get; set; }

        [Required(ErrorMessage = "La Acción es requerida.")]
        [MaxLength(20, ErrorMessage = "La Acción no puede exceder los 20 caracteres.")]
        public string Accion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Fecha y Hora son requeridas.")]
        public DateTime Fecha_Hora { get; set; }

        [MaxLength(500, ErrorMessage = "El Comentario no puede exceder los 500 caracteres.")]
        public string Comentario { get; set; } = string.Empty;
    }
}


