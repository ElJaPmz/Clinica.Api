

using Clinica.Application.DTOs.Consultorio;


namespace Clinica.Application.Interface.Service
{
    public interface IConsultorioService
    {
        Task<ConsultorioDto?> ObtenerConsultorioPorIdAsync(int id);
        Task<IEnumerable<ConsultorioDto>> ObtenerTodosAsync(int pagina, int tamanioPagina);
        Task<IEnumerable<ConsultorioDto>> BuscarConsultorios(string valor, int pagina, int tamanioPagina);
        Task<int> ContarTodasAsync();
        Task<int> ContarConsultoriosPorBusquedaAsync(string valor);

        Task<ConsultorioDto> CrearAsync(ConsultorioCrearDto dto);
        Task<ConsultorioDto> ActualizarAsync( int id,ConsultorioActualizarDto dto);
        Task EliminarAsync(int id);
    }
}
