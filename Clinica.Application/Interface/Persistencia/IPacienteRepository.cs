using System;
using System.Collections.Generic;
using System.Text;

using Clinica.Domain.Entities;

namespace Clinica.Application.Interface.Persistencia
{
    public interface IPacienteRepository
    {
        Task<List<Paciente>> GetAllAsync();

        Task<Paciente?> GetByIdAsync(int id);

        Task AddAsync(Paciente paciente);

        Task UpdateAsync(Paciente paciente);

        Task DeleteAsync(int id);
    }
}