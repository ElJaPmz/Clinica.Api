using System;
using System.Collections.Generic;
using System.Text;
namespace Clinica.Domain.Entities
{
    public class Usuario
    {
        public int Id_Usuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = "Medico";

        // Campo opcional para vincular con la tabla Medicos
        public int? Id_Medico { get; set; }
    }
}