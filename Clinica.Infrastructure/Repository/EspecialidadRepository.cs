using Clinica.Domain.Entities;
using Clinica.Application.Interface.Persistencia;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica.Infrastructure.Repository
{
    public class EspecialidadRepository : IEspecialidadRepository
    {
        private readonly ApplicationDbContext _context;

        public EspecialidadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Especialidad?> ObtenerPorIdAsync(int id)
        {
            return await _context.Especialidades
                .FirstOrDefaultAsync(e => e.Id_Especialidad == id);
        }

        public async Task<IEnumerable<Especialidad>> ObtenerTodosAsync(int pagina, int tamanioPagina)
        {
            return await _context.Especialidades
                .OrderBy(e => e.Id_Especialidad)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<IEnumerable<Especialidad>> BuscarAsync(string valor, int pagina, int tamanioPagina)
        {
            var query = _context.Especialidades.AsQueryable();

            query = query.Where(e =>
                e.Nombre_Especialidad.ToLower().Contains(valor.ToLower())
            );

            return await query
                .OrderBy(e => e.Id_Especialidad)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<int> ContarTodasAsync()
        {
            return await _context.Especialidades.CountAsync();
        }

        public async Task<int> ContarPorBusquedaAsync(string valor)
        {
            return await _context.Especialidades
                .Where(e => e.Nombre_Especialidad.ToLower().Contains(valor.ToLower()))
                .CountAsync();
        }

        public async Task CrearAsync(Especialidad especialidad)
        {
            await _context.Especialidades.AddAsync(especialidad);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Especialidad especialidad)
        {
            _context.Especialidades.Update(especialidad);
            await _context.SaveChangesAsync();
        }

        public async Task<int> EliminarAsync(int id)
        {
            return await _context.Especialidades
                .Where(e => e.Id_Especialidad == id)
                .ExecuteDeleteAsync();
        }
    }
}