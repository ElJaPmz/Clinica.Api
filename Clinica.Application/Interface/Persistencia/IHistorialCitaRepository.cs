using Clinica.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinica.Application.Interface.Persistencia
{
    public interface IHistorialCitaRepository
    {
        // Obtener todos los registros (para auditoría general)
        Task<IEnumerable<HistorialCita>> GetAllAsync();

        // Obtener historial de una cita específica (muy útil para tu proyecto)
        Task<IEnumerable<HistorialCita>> GetByCitaIdAsync(int idCita);

        // Crear un nuevo registro
        Task<bool> CreateAsync(HistorialCita historial);

        // Actualizar (siguiendo tu estándar de CRUD completo)
        Task<bool> UpdateAsync(HistorialCita historial);

        // Eliminar (opcional, pero para completar el CRUD)
        Task<bool> DeleteAsync(int id);
    }
}
