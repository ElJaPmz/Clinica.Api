using Clinica.Application.DTOs.Cita;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Service
{
    public interface ICitaService
    {
        Task<IEnumerable<CitaDto>> ObtenerTodasAsync();
        Task<CitaDto?> ObtenerPorIdAsync(int id);
        Task<CitaDto> CrearAsync(CitaCrearDto dto);
        Task<bool> ActualizarAsync(int id, CitaActualizarDto dto);
        Task<bool> EliminarAsync(int id);

        // Métodos adicionales para la lógica de negocio
        Task<IEnumerable<CitaDto>> ObtenerCitasPorMedicoAsync(int idMedico);
        Task<IEnumerable<CitaDto>> ObtenerCitasPorPacienteAsync(int idPaciente);
    }
}
