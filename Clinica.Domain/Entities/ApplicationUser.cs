using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Clinica.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
       
        public string NombreCompleto { get; set; } = null!;
        public bool Activo { get; set; } = true;

        // Adaptación para tu Clínica: 
        // Relación con tu tabla de Citas 
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();

        // Propiedad adicional opcional: Si quieres vincular el usuario 
        // directamente con un médico de tu tabla Médicos
        public int? Id_Medico { get; set; }
    }
}
