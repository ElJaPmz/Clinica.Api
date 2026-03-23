using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.DTOs.Paciente
{
    public class PacienteActualizarDto
    {
        public int IdPaciente { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public DateTime FechaNacimiento { get; set; }

        public string Telefono { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string Cedula { get; set; } = null!;

        public string TipoPaciente { get; set; } = null!;
    }
}
