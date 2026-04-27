
using Clinica.Application.Interface.Persistencia;
using Clinica.Domain.Entities;
using Clinica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Infrastructure.Persistencia
{
    public class ClinicaRepository : IClinicaRepository
    {
        private readonly ApplicationDbContext _context;

        public ClinicaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClinicaPerfil?> ObtenerInformacionAsync()
        {
            return await _context.Clinicas.FirstOrDefaultAsync();
        }

        public async Task<bool> ActualizarAsync(ClinicaPerfil clinica)
        {
            _context.Clinicas.Update(clinica);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CrearAsync(ClinicaPerfil clinica)
        {
            await _context.Clinicas.AddAsync(clinica);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}