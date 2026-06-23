using System;
using System.Collections.Generic;
using System.Text;
using Clinica.Application.Interface.Persistencia;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        public DashboardRepository(ApplicationDbContext context) => _context = context;

        public async Task<int> GetCountPacientes() => await _context.Pacientes.CountAsync();
        public async Task<int> GetCountMedicos() => await _context.Medicos.CountAsync();
        public async Task<int> GetCountCitas() => await _context.Citas.CountAsync();
        public async Task<int> GetCountConsultorios() => await _context.Consultorios.CountAsync();
    }
}
