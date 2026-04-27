using Clinica.Application.DTOs.ClinicaPerfil;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Service
{
    public interface IClinicaService
    {
        Task<bool> CrearPerfilAsync(ClinicaActualizarDto dto);

        // Para el GET público
        Task<ClinicaDto?> ObtenerPerfilAsync();

        // Para el PUT administrativo
        Task<bool> ActualizarPerfilAsync(ClinicaActualizarDto dto);
    }
}
