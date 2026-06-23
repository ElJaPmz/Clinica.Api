using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace Clinica.Domain.Entities
{
    public class Paciente
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

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();

    }
}