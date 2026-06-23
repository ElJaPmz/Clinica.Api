using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Persistencia
{
    public interface IDashboardRepository
    {
        Task<int> GetCountPacientes();
        Task<int> GetCountMedicos();
        Task<int> GetCountCitas();
        Task<int> GetCountConsultorios();
    }
}