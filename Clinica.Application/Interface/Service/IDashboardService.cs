using System;
using System.Collections.Generic;
using System.Text;
using Clinica.Application.DTOs;

namespace Clinica.Application.Interface
{
    public interface IDashboardService
    {
        Task<DashboardDto> ObtenerResumenEstrategico();
    }
}