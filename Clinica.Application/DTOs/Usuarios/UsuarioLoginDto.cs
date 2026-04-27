using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Clinica.Application.DTOs.Usuarios
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio para iniciar sesión.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria para iniciar sesión.")]
        public string Password { get; set; } = string.Empty;
        // Agrega esta línea para que aparezca en el JSON de Swagger
        [Required(ErrorMessage = "El rol es requerido para validar el acceso.")]
        public string Rol { get; set; } = string.Empty;
    }
}
