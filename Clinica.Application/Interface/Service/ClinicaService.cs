using AutoMapper; // Necesario para el mapeo automático
using Clinica.Application.DTOs.ClinicaPerfil;
using Clinica.Application.Interface.Persistencia;
using Clinica.Application.Interface.Service;
using Clinica.Domain.Entities;

namespace Clinica.Application.Services
{
    public class ClinicaService : IClinicaService
    {
        private readonly IClinicaRepository _clinicaRepository;
        private readonly IMapper _mapper; // Inyectamos el Mapper

        public ClinicaService(IClinicaRepository clinicaRepository, IMapper mapper)
        {
            _clinicaRepository = clinicaRepository;
            _mapper = mapper;
        }

        public async Task<bool> CrearPerfilAsync(ClinicaActualizarDto dto)
        {
            // Mapeamos el DTO a la Entidad
            var nuevaClinica = _mapper.Map<ClinicaPerfil>(dto);

            // Llamamos al repositorio que ya tenía el método CrearAsync
            return await _clinicaRepository.CrearAsync(nuevaClinica);
        }
        public async Task<ClinicaDto?> ObtenerPerfilAsync()
        {
            var clinica = await _clinicaRepository.ObtenerInformacionAsync();

            if (clinica == null) return null;

            // Usamos AutoMapper en lugar de la asignación manual
            return _mapper.Map<ClinicaDto>(clinica);
        }

        public async Task<bool> ActualizarPerfilAsync(ClinicaActualizarDto dto)
        {
            var clinicaExistente = await _clinicaRepository.ObtenerInformacionAsync();

            if (clinicaExistente == null) return false;

            // Mapeamos los cambios del DTO a la entidad existente
            // Esto actualiza automáticamente Nombre, Descripcion, etc.
            _mapper.Map(dto, clinicaExistente);

            return await _clinicaRepository.ActualizarAsync(clinicaExistente);
        }
    }
}