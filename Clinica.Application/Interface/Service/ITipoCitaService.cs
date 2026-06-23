using Clinica.Application.DTOs.TipoCita;

namespace Clinica.Application.Interface.Service
{
    public interface ITipoCitaService
    {
        Task<IEnumerable<TipoCitaDto>> ObtenerTodosAsync();
        Task<IEnumerable<TipoCitaDto>> ObtenerPorEspecialidadAsync(int idEspecialidad);
    }
}
