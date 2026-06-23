using Clinica.Application.DTOs.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Response
{
    public class LoginRespuestaUsuarioDto
    {
        public UsuarioDto? Usuario { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
