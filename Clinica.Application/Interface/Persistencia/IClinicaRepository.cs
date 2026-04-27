using Clinica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Persistencia
{
    public interface IClinicaRepository
    {
        // Método para obtener la información (usado por el GET público)
        Task<ClinicaPerfil?> ObtenerInformacionAsync();

        // Método para actualizar la información (usado por el PUT administrativo)
        Task<bool> ActualizarAsync(ClinicaPerfil clinica);

        // Opcional: Si decides hacer el primer registro por código
        Task<bool> CrearAsync(ClinicaPerfil clinica);
    }
}
