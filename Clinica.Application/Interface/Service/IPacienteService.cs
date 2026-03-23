using System;
using System.Collections.Generic;
using System.Text;

using Clinica.Application.DTOs.Paciente;

namespace Clinica.Application.Interface.Service
{
    public interface IPacienteService
    {
        Task<List<PacienteDto>> GetAllAsync();

        Task<PacienteDto?> GetByIdAsync(int id);

        Task AddAsync(PacienteCrearDto dto);

        Task UpdateAsync(PacienteActualizarDto dto);

        Task DeleteAsync(int id);
    }
}