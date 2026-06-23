using Clinica.Domain.Entities;

namespace Clinica.Application.Interface.Persistencia
{
    public interface ITipoCitaRepository
    {
        Task<IEnumerable<TipoCita>> ObtenerTodosAsync();
        Task<IEnumerable<TipoCita>> ObtenerPorEspecialidadAsync(int idEspecialidad);
        Task<TipoCita?> ObtenerPorNombreYEspecialidadAsync(string nombre, int idEspecialidad);
    }
}
