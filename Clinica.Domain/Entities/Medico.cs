using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Domain.Entities
{
    public class Medico
    {

        public int Id_Medico { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int Id_Especialidad { get; set; }
        public int telefono { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;


        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();

        // Navegación
        public virtual Especialidad Especialidad { get; set; } = null!;





    }
}
