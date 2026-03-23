using Clinica.Application.DTOs.Paciente;

namespace Clinica.Application.Interface.Service
{
    public interface IPacienteService
    {
        Task<List<PacienteDto>> ObtenerTodosAsync(int pagina, int tamanioPagina);

        Task<int> ContarTodosAsync();

        Task<List<PacienteDto>> BuscarPacientes(string valor, int pagina, int tamanioPagina);

        Task<int> ContarPacientesPorBusquedaAsync(string valor);

        Task<PacienteDto?> ObtenerPacientePorIdAsync(int id);

        Task<PacienteDto> CrearAsync(PacienteCrearDto dto);

        Task ActualizarAsync(int id, PacienteActualizarDto dto);

        Task EliminarAsync(int id);
    }
}