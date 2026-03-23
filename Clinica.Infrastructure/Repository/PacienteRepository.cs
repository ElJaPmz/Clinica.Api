using System;
using System.Collections.Generic;
using System.Text;

using Clinica.Domain.Entities;
using Clinica.Application.Interface.Persistencia;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica.Infrastructure.Repository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly ApplicationDbContext _context;

        public PacienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Paciente?> ObtenerPacientePorIdAsync(int id)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.IdPaciente == id);
        }

        public async Task<IEnumerable<Paciente>> ObtenerTodosAsync(int pagina, int tamanioPagina)
        {
            return await _context.Pacientes
                .OrderBy(p => p.IdPaciente)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<IEnumerable<Paciente>> BuscarPacientes(string valor, int pagina, int tamanioPagina)
        {
            var query = _context.Pacientes.AsQueryable();

            query = query.Where(p =>
                p.Nombre.ToLower().Contains(valor.ToLower()) ||
                p.Apellido.ToLower().Contains(valor.ToLower()) ||
                p.Cedula.Contains(valor)
            );

            return await query
                .OrderBy(p => p.IdPaciente)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<int> ContarTodosAsync()
        {
            return await _context.Pacientes.CountAsync();
        }

        public async Task<int> ContarPacientesPorBusquedaAsync(string valor)
        {
            return await _context.Pacientes
                .Where(p =>
                    p.Nombre.ToLower().Contains(valor.ToLower()) ||
                    p.Apellido.ToLower().Contains(valor.ToLower()) ||
                    p.Cedula.Contains(valor)
                )
                .CountAsync();
        }

        public async Task CrearAsync(Paciente paciente)
        {
            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Paciente paciente)
        {
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task<int> EliminarAsync(int id)
        {
            return await _context.Pacientes
                .Where(p => p.IdPaciente == id)
                .ExecuteDeleteAsync();
        }
    }
}
