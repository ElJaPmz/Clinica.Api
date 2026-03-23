using System;
using System.Collections.Generic;
using System.Text;

using Clinica.Application.DTOs.Paciente;
using Clinica.Application.Interface.Persistencia;
using Clinica.Application.Interface.Service;
using Clinica.Domain.Entities;

namespace Clinica.Application.Service
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PacienteDto>> GetAllAsync()
        {
            var pacientes = await _repository.GetAllAsync();

            return pacientes.Select(p => new PacienteDto
            {
                IdPaciente = p.IdPaciente,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                FechaNacimiento = p.FechaNacimiento,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion,
                Cedula = p.Cedula,
                TipoPaciente = p.TipoPaciente
            }).ToList();
        }

        public async Task<PacienteDto?> GetByIdAsync(int id)
        {
            var p = await _repository.GetByIdAsync(id);

            if (p == null) return null;

            return new PacienteDto
            {
                IdPaciente = p.IdPaciente,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                FechaNacimiento = p.FechaNacimiento,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion,
                Cedula = p.Cedula,
                TipoPaciente = p.TipoPaciente
            };
        }

        public async Task AddAsync(PacienteCrearDto dto)
        {
            var paciente = new Paciente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                FechaNacimiento = dto.FechaNacimiento,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Direccion = dto.Direccion,
                Cedula = dto.Cedula,
                TipoPaciente = dto.TipoPaciente
            };

            await _repository.AddAsync(paciente);
        }

        public async Task UpdateAsync(PacienteActualizarDto dto)
        {
            var paciente = new Paciente
            {
                IdPaciente = dto.IdPaciente,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                FechaNacimiento = dto.FechaNacimiento,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Direccion = dto.Direccion,
                Cedula = dto.Cedula,
                TipoPaciente = dto.TipoPaciente
            };

            await _repository.UpdateAsync(paciente);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
