using Clinica.Application.DTOs.Especialidad;

namespace Clinica.Application.Interface.Service
{
    public interface IEspecialidadService
    {
        Task<EspecialidadDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<EspecialidadDto>> ObtenerTodosAsync(int pagina, int tamanioPagina);
        Task<IEnumerable<EspecialidadDto>> BuscarAsync(string valor, int pagina, int tamanioPagina);
        Task<int> ContarTodasAsync();
        Task<int> ContarPorBusquedaAsync(string valor);

        Task<EspecialidadDto> CrearAsync(EspecialidadCrearDto dto);
        Task<EspecialidadDto> ActualizarAsync(int id, EspecialidadActualizarDto dto);
        Task EliminarAsync(int id);
    }
}