using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinica.Application.Interface.Persistencia;
using Clinica.Domain.Entities;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repository
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Medico?> ObtenerMedicoPorIdAsync(int id)
        {
            return await _context.Medicos
                // BORRA EL .Include(...) AQUÍ
                .FirstOrDefaultAsync(m => m.Id_Medico == id);
        }

        public async Task<IEnumerable<Medico>> ObtenerTodosAsync(int pagina, int tamanioPagina)
        {
            return await _context.Medicos
                .AsNoTracking()
                // Borramos el .Include que está dando guerra
                .OrderBy(m => m.Id_Medico)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<IEnumerable<Medico>> BuscarMedicosAsync(string valor, int pagina, int tamanioPagina)
        {
            // 1. Iniciamos la consulta sobre la tabla Medicos
            var query = _context.Medicos.AsNoTracking().AsQueryable();

            // 2. Aplicamos el filtro de búsqueda solo si hay un valor
            if (!string.IsNullOrEmpty(valor))
            {
                var busqueda = valor.Trim().ToLower();
                query = query.Where(m =>
                    m.Nombre.ToLower().Contains(busqueda) ||
                    m.Apellido.ToLower().Contains(busqueda)
                );
            }

            // 3. Ejecutamos la paginación sobre la variable 'query' (la que ya tiene el filtro)
            return await query
                .OrderBy(m => m.Id_Medico)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        public async Task<int> BuscarTodosAsync()
        {
            return await _context.Medicos
                .CountAsync();
                //.ConfigureAwait(false);
        }

        public async Task<int> ContarMedicosPorBusquedaAsync(string valor)
        {
            var query = _context.Medicos
                .AsNoTracking();

            if (!string.IsNullOrEmpty(valor))
            {
                //query = query.Where(m => m.Nombre.Contains(valor) || m.Apellido.Contains(valor));
                var busqueda = valor.Trim().ToLower();

                query = query.Where(m => m.Nombre.ToLower().Contains(busqueda) || m.Apellido.ToLower().Contains(busqueda));
            }

            return await query.CountAsync();
        }

        public async Task CrearAsync(Medico medico)
        {
            await _context.Medicos.AddAsync(medico);
                //.ConfigureAwait(false);
            await _context.SaveChangesAsync();
                //.ConfigureAwait(false);
        }

        public async Task ActualizarAsync(Medico medico)
        {
            _context.Medicos.Update(medico);
            await _context.SaveChangesAsync();
                //.ConfigureAwait(false);
        }

        public async Task<int> EliminarAsync(int id)
        {
            return await _context.Medicos
                .Where(m => m.Id_Medico == id)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Medico>> ObtenerMedicosPorEspecialidadAsync(int idEspecialidad)
        {
            return await _context.Medicos
                .AsNoTracking()
                .Where(m => m.Id_Especialidad == idEspecialidad && m.Estado != "Inactivo")
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }
    }
}