using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.DTOs.Usuarios
{
    public class UsuarioDto
    {
        public int Id_Usuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public int? Id_Medico { get; set; }
    }
}