using Clinica.Application.Interface.Persistencia;
using Clinica.Domain.Entities;
using Clinica.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repository
{
    public class CitaRepository : ICitaRepository
    {
        private readonly ApplicationDbContext _context;

        public CitaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cita>> ObtenerTodasAsync()
        {
            return await _context.Citas
                .Include(c => c.Paciente)    
                .Include(c => c.Medico)      
                .Include(c => c.Consultorio) 
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Cita?> ObtenerPorIdAsync(int id)
        {
            return await _context.Citas
                    .Include(c => c.Paciente)    
                    .Include(c => c.Medico)      
                    .Include(c => c.Consultorio) 
                    .FirstOrDefaultAsync(c => c.IdCita == id);
        }

        public async Task CrearAsync(Cita cita)
        {
            await _context.Set<Cita>().AddAsync(cita);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Cita cita)
        {
            _context.Set<Cita>().Update(cita);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var cita = await ObtenerPorIdAsync(id);
            if (cita != null)
            {
                _context.Set<Cita>().Remove(cita);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Cita>> ObtenerCitasPorMedicoAsync(int idMedico)
        {
            return await _context.Citas
                .Include(c => c.Paciente)    // Para que salga el nombre del paciente en la agenda del doc
                .Include(c => c.Medico)      // Para confirmar el nombre del médico
                .Include(c => c.Consultorio) // Para saber dónde atenderá
                .Where(c => c.IdMedico == idMedico)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> ObtenerCitasPorPacienteAsync(int idPaciente)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)      // Para que el paciente sepa quién lo atiende
                .Include(c => c.Consultorio) // Para que el paciente sepa a qué sala ir
                .Where(c => c.IdPaciente == idPaciente)
                .ToListAsync();
        }
    }
} 

