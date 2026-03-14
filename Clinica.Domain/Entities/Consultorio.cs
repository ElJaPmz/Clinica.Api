using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Domain.Entities
{
    public class Consultorio
    {
        public int Id_Consultorio {  get; set; }
        public int NumeroConsultorio { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
