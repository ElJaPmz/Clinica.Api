using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Clinica.Application.DTOs.Especialidad;
using Clinica.Application.Interface.Persistencia;
using Clinica.Domain.Entities;

namespace Clinica.Application.Interface.Service
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly IEspecialidadRepository _especialidadRepository;
        private readonly IMapper _mapper;

        public EspecialidadService(IEspecialidadRepository especialidadRepository, IMapper mapper)
        {
            _especialidadRepository = especialidadRepository;
            _mapper = mapper;
        }

        public async Task<EspecialidadDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El id debe ser mayor a cero.");

            var especialidad = await _especialidadRepository.ObtenerPorIdAsync(id);

            if (especialidad is null)
                throw new KeyNotFoundException($"No se encontró la especialidad con id {id}.");

            return _mapper.Map<EspecialidadDto>(especialidad);
        }

        public async Task<IEnumerable<EspecialidadDto>> ObtenerTodosAsync(int pagina, int tamanioPagina)
        {
            var especialidades = await _especialidadRepository.ObtenerTodosAsync(pagina, tamanioPagina);
            return _mapper.Map<IEnumerable<EspecialidadDto>>(especialidades);
        }

        public async Task<IEnumerable<EspecialidadDto>> BuscarAsync(string valor, int pagina, int tamanioPagina)
        {
            var especialidades = await _especialidadRepository.BuscarAsync(valor, pagina, tamanioPagina);
            return _mapper.Map<IEnumerable<EspecialidadDto>>(especialidades);
        }

        public async Task<int> ContarTodasAsync()
        {
            return await _especialidadRepository.ContarTodasAsync();
        }

        public async Task<int> ContarPorBusquedaAsync(string valor)
        {
            return await _especialidadRepository.ContarPorBusquedaAsync(valor);
        }

        public async Task<EspecialidadDto> CrearAsync(EspecialidadCrearDto dto)
        {
            if (dto == null)
                throw new ArgumentException("Los datos de la especialidad no pueden ser nulos.");

            var especialidad = _mapper.Map<Especialidad>(dto);

            await _especialidadRepository.CrearAsync(especialidad);

            return _mapper.Map<EspecialidadDto>(especialidad);
        }

        public async Task<EspecialidadDto> ActualizarAsync(int id, EspecialidadActualizarDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("El id debe ser mayor a cero.");

            var especialidad = await _especialidadRepository.ObtenerPorIdAsync(id);

            if (especialidad is null)
                throw new KeyNotFoundException($"Especialidad con id {id} no encontrada.");

            _mapper.Map(dto, especialidad);

            await _especialidadRepository.ActualizarAsync(especialidad);

            return _mapper.Map<EspecialidadDto>(especialidad);
        }

        public async Task EliminarAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El id debe ser mayor a cero.");

            int registrosBorrados = await _especialidadRepository.EliminarAsync(id);

            if (registrosBorrados == 0)
                throw new KeyNotFoundException($"No se encontró una especialidad con el ID {id}.");
        }
    }
}