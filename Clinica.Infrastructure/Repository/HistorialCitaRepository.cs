using Clinica.Application.Interface.Persistencia;
using Clinica.Domain.Entities;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica.Persistence.Repositories
{
    public class HistorialCitaRepository : IHistorialCitaRepository
    {
        private readonly ApplicationDbContext _context;

        public HistorialCitaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistorialCita>> GetAllAsync()
        {
            return await _context.HistorialCitas.ToListAsync();
        }

        public async Task<IEnumerable<HistorialCita>> GetByCitaIdAsync(int idCita)
        {
            return await _context.HistorialCitas
                .Where(h => h.Id_Cita == idCita)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(HistorialCita historial)
        {
            await _context.HistorialCitas.AddAsync(historial);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateAsync(HistorialCita historial)
        {
            _context.HistorialCitas.Update(historial);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var historial = await _context.HistorialCitas.FindAsync(id);
            if (historial == null) return false;

            _context.HistorialCitas.Remove(historial);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
