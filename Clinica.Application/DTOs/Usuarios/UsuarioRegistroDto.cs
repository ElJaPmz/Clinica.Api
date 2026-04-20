using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Clinica.Application.DTOs.Usuarios
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario no puede exceder los 50 caracteres.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido.")]
        public string Rol { get; set; } = "Medico";

        // Es opcional porque un Administrador podría no ser un Médico
        public int? Id_Medico { get; set; }
    }
}