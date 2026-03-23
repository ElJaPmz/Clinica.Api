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
                .Include(m => m.Id_Especialidad) // Asegura que la especialidad se cargue junto con el médico
                .FirstOrDefaultAsync(m => m.Id_Medico == id);
                //.ConfigureAwait(false);
        }

        public async Task<IEnumerable<Medico>> ObtenerTodosAsync(int pagina, int tamanioPagina)
        {
            return await _context.Medicos
                .AsNoTracking()
                .Include(m => m.Id_Especialidad)
                .OrderBy(m => m.Id_Medico)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
                //.ConfigureAwait(false);
        }

        public async Task<IEnumerable<Medico>> BuscarMedicosAsync(string valor, int pagina, int tamanioPagina)
        {
            var query = _context.Medicos
                .AsNoTracking()
                .Include(m => m.Id_Especialidad)
                .AsQueryable();
                //.Where(m => m.Nombre.Contains(valor) || m.Apellido.Contains(valor))
                //.OrderBy(m => m.Id_Medico);
            if(!string.IsNullOrEmpty(valor))
            {
                //query = query.Where(m => m.Nombre.Contains(valor) || m.Apellido.Contains(valor));
                var busqueda = valor.Trim().ToLower();

                query = query.Where(m => m.Nombre.ToLower().Contains(busqueda) || m.Apellido.ToLower().Contains(busqueda));
            }

            return await _context.Medicos
                .Include(m => m.Id_Especialidad)
                .OrderBy(m => m.Id_Medico)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
            //.Where(m => m.Nombre.Contains(valor) || m.Apellido.Contains(valor))
            //.OrderBy(m => m.Id_Medico)
            //.Skip(skip)
            //.Take(tamanioPagina)
            //.ToListAsync()
            //.ConfigureAwait(false);
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
            //.ConfigureAwait(false);
        }
    }
}