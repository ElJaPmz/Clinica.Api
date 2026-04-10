using Clinica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Persistencia
{
    public interface IEspecialidadRepository
    {
        Task<Especialidad?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Especialidad>> ObtenerTodosAsync(int pagina, int tamanioPagina);
        Task<IEnumerable<Especialidad>> BuscarAsync(string valor, int pagina, int tamanioPagina);
        Task<int> ContarTodasAsync();
        Task<int> ContarPorBusquedaAsync(string valor);
        Task CrearAsync(Especialidad especialidad);
        Task ActualizarAsync(Especialidad especialidad);
        Task<int> EliminarAsync(int id);
    }
}
