using Clinica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Persistencia
{
    public interface ICitaRepository
    {
        // CRUD Básico
        Task<IEnumerable<Cita>> ObtenerTodasAsync();
        Task<Cita?> ObtenerPorIdAsync(int id);
        Task CrearAsync(Cita cita);
        Task ActualizarAsync(Cita cita);
        Task EliminarAsync(int id);

        // Métodos específicos para Citas (Muy útiles para un sistema clínico)
        Task<IEnumerable<Cita>> ObtenerCitasPorMedicoAsync(int idMedico);
        Task<IEnumerable<Cita>> ObtenerCitasPorPacienteAsync(int idPaciente);
    }
}
