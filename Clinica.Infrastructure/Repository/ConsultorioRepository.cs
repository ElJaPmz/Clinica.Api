using Clinica.Domain.Entities;
using Clinica.Application.Interface.Persistencia;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinica.Infrastructure.Repository
{
    public class ConsultorioRepository : IConsultorioRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultorioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Consultorio?> ObtenerConsultorioPorIdAsync(int id)
        {
            return await _context.Consultorios
                .FirstOrDefaultAsync(c => c.Id_Consultorio == id);
        }

        public async Task<IEnumerable<Consultorio>> ObtenerTodosAsync(int pagina, int tamanioPagina)
        {
            return await _context.Consultorios
                .OrderBy(c => c.Id_Consultorio)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<IEnumerable<Consultorio>> BuscarConsultorios(string valor, int pagina, int tamanioPagina)
        {
            var query = _context.Consultorios.AsQueryable();

            query = query.Where(c => c.Descripcion.ToLower().Contains(valor.ToLower())||
                                    c.NumeroConsultorio.ToString().Contains(valor));

            return await query
                .OrderBy(c => c.Id_Consultorio)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<int> ContarTodasAsync()
        {
            return await _context.Consultorios.CountAsync();
        }

        public async Task<int> ContarConsultoriosPorBusquedaAsync(string valor)
        {
            return await _context.Consultorios
                .Where(c => c.Descripcion.ToLower().Contains(valor.ToLower()) ||
                            c.NumeroConsultorio.ToString().Contains(valor))
                .CountAsync();
        }

        public async Task CrearAsync(Consultorio consultorio)
        {
            await _context.Consultorios.AddAsync(consultorio);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Consultorio consultorio)
        {
            _context.Consultorios.Update(consultorio);
            await _context.SaveChangesAsync();
        }

        public async Task<int> EliminarAsync(int id)
        {
            return await _context.Consultorios
                .Where(c => c.Id_Consultorio == id)
                .ExecuteDeleteAsync();
        }
    }
}
