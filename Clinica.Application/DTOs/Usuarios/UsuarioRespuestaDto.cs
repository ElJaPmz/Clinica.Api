using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.DTOs.Usuarios
{
    public class UsuarioRespuestaDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}