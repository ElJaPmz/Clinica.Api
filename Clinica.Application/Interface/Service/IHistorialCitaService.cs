using Clinica.Application.DTOs.HistorialCita;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinica.Application.Interface.Service
{
    public interface IHistorialCitaService
    {
        Task<IEnumerable<HistorialCitaDto>> GetAllAsync();
        Task<IEnumerable<HistorialCitaDto>> GetByCitaIdAsync(int idCita);
        Task<bool> CreateAsync(HistorialCitaCrearDto crearDto);
        Task<bool> UpdateAsync(HistorialCitaActualizarDto actualizarDto);
        Task<bool> DeleteAsync(int id);
    }
}