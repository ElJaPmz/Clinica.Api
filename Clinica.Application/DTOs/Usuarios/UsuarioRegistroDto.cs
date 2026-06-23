using System.ComponentModel.DataAnnotations;

namespace Clinica.Application.DTOs.Usuarios
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage = "El nombre completo es requerido.")]
        public string NombreCompleto { get; set; } = null!;

        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El rol es requerido.")]
        public string Rol { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es requerido.")]
        public string PhoneNumber { get; set; } = null!;

        public bool Activo { get; set; } = true;

        // Adaptado a tu esquema: Para vincular al usuario con un médico existente
        public int? Id_Medico { get; set; }
    }
}
