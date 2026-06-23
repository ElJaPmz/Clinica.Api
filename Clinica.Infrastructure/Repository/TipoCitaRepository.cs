using Clinica.Application.Interface.Persistencia;
using Clinica.Domain.Entities;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repository
{
    public class TipoCitaRepository : ITipoCitaRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoCitaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoCita>> ObtenerTodosAsync()
        {
            return await _context.TipoCitas
                .AsNoTracking()
                .OrderBy(t => t.Id_Especialidad)
                .ThenBy(t => t.Nombre_TipoCita)
                .ToListAsync();
        }

        public async Task<IEnumerable<TipoCita>> ObtenerPorEspecialidadAsync(int idEspecialidad)
        {
            return await _context.TipoCitas
                .AsNoTracking()
                .Where(t => t.Id_Especialidad == idEspecialidad)
                .OrderBy(t => t.Nombre_TipoCita)
                .ToListAsync();
        }

        public async Task<TipoCita?> ObtenerPorNombreYEspecialidadAsync(string nombre, int idEspecialidad)
        {
            return await _context.TipoCitas
                .AsNoTracking()
                .FirstOrDefaultAsync(t =>
                    t.Id_Especialidad == idEspecialidad &&
                    t.Nombre_TipoCita.ToLower() == nombre.ToLower());
        }
    }
}
