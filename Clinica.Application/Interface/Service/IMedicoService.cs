using Clinica.Application.DTOs.Medico;
using Clinica.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Interface.Service
{
    public interface IMedicoService
    {
        //Metoedos de consulta
        Task<MedicoDto?> ObtenerMedicoPorIdAsync(int id);
        Task<IEnumerable<MedicoDto>> ObtenerTodosAsync(int pagina, int tamanioPagina);
        Task<IEnumerable<MedicoDto>> BuscarMedicosAsync(string valor, int pagina, int tamanioPagina);
        Task<int> BuscarTodosAsync();
        Task<int> ContarMedicosPorBusquedaAsync(string valor);
        Task<IEnumerable<MedicoDto>> ObtenerMedicosPorEspecialidadAsync(int idEspecialidad);

        //Metodos de modificacion
        Task<MedicoDto> CrearAsync(MedicoCrearDto dto);
        Task<MedicoDto> ActualizarAsync(int id, MedicoActualizarDto dto);
        Task EliminarAsync(int id);
    }
}
