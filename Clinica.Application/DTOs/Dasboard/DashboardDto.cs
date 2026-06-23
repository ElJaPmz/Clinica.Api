using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.DTOs
{
    public class DashboardDto
    {
        public int TotalPacientes { get; set; }
        public int TotalMedicos { get; set; }
        public int TotalCitas { get; set; }
        public int TotalConsultorios { get; set; }
    }
}
