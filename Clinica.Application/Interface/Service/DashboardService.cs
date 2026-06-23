using Clinica.Application.DTOs;
using Clinica.Application.Interface;
using Clinica.Application.Interface.Persistencia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Infrastructure.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        public DashboardService(IDashboardRepository repository) => _repository = repository;

        public async Task<DashboardDto> ObtenerResumenEstrategico()
        {
            return new DashboardDto
            {
                TotalPacientes = await _repository.GetCountPacientes(),
                TotalMedicos = await _repository.GetCountMedicos(),
                TotalCitas = await _repository.GetCountCitas(),
                TotalConsultorios = await _repository.GetCountConsultorios()
            };
        }
    }
}