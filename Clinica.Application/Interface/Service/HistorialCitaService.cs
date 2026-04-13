using AutoMapper;
using Clinica.Application.DTOs.HistorialCita;
using Clinica.Application.Interface.Persistencia;
using Clinica.Application.Interface.Service;
using Clinica.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinica.Application.Service
{
    public class HistorialCitaService : IHistorialCitaService
    {
        private readonly IHistorialCitaRepository _repository;
        private readonly IMapper _mapper;

        public HistorialCitaService(IHistorialCitaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HistorialCitaDto>> GetAllAsync()
        {
            var historial = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<HistorialCitaDto>>(historial);
        }

        public async Task<IEnumerable<HistorialCitaDto>> GetByCitaIdAsync(int idCita)
        {
            var historial = await _repository.GetByCitaIdAsync(idCita);
            return _mapper.Map<IEnumerable<HistorialCitaDto>>(historial);
        }

        public async Task<bool> CreateAsync(HistorialCitaCrearDto crearDto)
        {
            var entidad = _mapper.Map<HistorialCita>(crearDto);

            // Esto arregla el error de PostgreSQL forzando la fecha a UTC
            entidad.Fecha_Hora = DateTime.SpecifyKind(crearDto.Fecha_Hora, DateTimeKind.Utc);

            return await _repository.CreateAsync(entidad);
        }

        public async Task<bool> UpdateAsync(HistorialCitaActualizarDto actualizarDto)
        {
            var entidad = _mapper.Map<HistorialCita>(actualizarDto);
            // Aseguramos que el ID se mantenga para la actualización
            entidad.Id_Historial = actualizarDto.Id_Historial;
            return await _repository.UpdateAsync(entidad);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
